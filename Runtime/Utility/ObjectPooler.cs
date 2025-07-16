// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public struct PooledObject
    {
        [SerializeField] public GameObject m_prefab;
        [SerializeField] public int m_numberOfObjects;
    }

    public class ObjectPooler : MonoBehaviour
    {
        enum PoolerBehavior
        {
            RecycleOldest,
            DoubleSize,
            Warn
        }
        
        [Tooltip("The behavior of the pooler when it runs out of objects. " +
                 "Recycle Oldest will recycle the oldest object in the pool, " +
                 "Double Size will double the size of the pool (which is generally not ideal since it can cause GC spikes), " +
                 "Warn will log a warning that the pooler is out of objects and do nothing.")]
        [SerializeField] PoolerBehavior m_poolerBehavior = PoolerBehavior.RecycleOldest;
        
        List<GameObject> m_freeList = new();
        public List<GameObject> FreeList => m_freeList;
        List<GameObject> m_usedList = new();
        public List<GameObject> UsedList => m_usedList;
        
        int NumFree => m_freeList.Count;

        /// <summary>
        /// GenerateObjects() takes a list of PooledObject structs and instantiates GameObjects for use in the shared pool.
        /// Each PooledObject struct contains the GameObject that should be instantiated and an int that represents the number of those GameObjects that should be available in the shared pool.
        /// </summary>
        public void GenerateObjects(List<PooledObject> pooledObjects, bool setParent = true)
        {
			// Destroy all the old GameObjects
            ClearAll();

            // Create the new GameObjects
            for (int i=0; i<pooledObjects.Count; i++)
            {
                for (int j=0; j<pooledObjects[i].m_numberOfObjects; j++)
                {
                    GameObject go;
                    if (setParent)
                        go = Instantiate(pooledObjects[i].m_prefab, Vector3.zero, Quaternion.identity, transform);
                    else
                        go = Instantiate(pooledObjects[i].m_prefab, Vector3.zero, Quaternion.identity);
                    
                    go.SetActive(false);

                    // Add a pooler identifier to the GameObject so that it can be referenced without a parent Pooler.
                    go.AddComponent<PoolerIdentifier>().m_pooler = this;

                    m_freeList.Add(go);
                }
            }
        }

        public void GenerateObjects(PooledObject pooledObject, bool setParent = true)
        {
            var pooledObjects = new List<PooledObject> { pooledObject };
            GenerateObjects(pooledObjects, setParent);
        }

        /// <summary>
        /// Retrieve() returns a GameObject from the shared pool at the desired position and rotation.
        /// If there's more than one type of GameObject in the shared pool, it will choose randomly.
        /// </summary>
        public GameObject Retrieve(Vector3 position = default, Quaternion rotation = default, bool setLocalRotation = false)
        {
            if (NumFree == 0)
            {
                switch (m_poolerBehavior)
                {
                    case PoolerBehavior.RecycleOldest:
                        Recycle(m_usedList[0]);
                        break;
                    case PoolerBehavior.DoubleSize:
                        Debug.LogWarning($"Object pooler attached to the GameObject \"{gameObject.name}\" is out of objects. Doubling the size of the pool. " +
                                  $"Note: This is generally not ideal behavior because it instantiates objects at runtime which can cause GC spikes.", gameObject);
                        foreach (var t in m_usedList)
                        {
                            GameObject go = Instantiate(t, Vector3.zero, Quaternion.identity, transform);
                            go.name = t.name; // Remove the repeating "(Clone)" suffix
                            go.SetActive(false);
                            go.AddComponent<PoolerIdentifier>().m_pooler = this;
                            m_freeList.Add(go);
                        }
                        break;
                    case PoolerBehavior.Warn:
                        Debug.LogWarning($"Object pooler attached to the GameObject \"{gameObject.name}\" is out of objects!", gameObject);
                        return null;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Pull a GameObject from the end of the free list
            GameObject pooledObject = m_freeList[NumFree - 1];
            m_freeList.RemoveAt(NumFree - 1);
            m_usedList.Add(pooledObject);

            // Set the position and rotation
            pooledObject.transform.position = position;
            pooledObject.transform.rotation = rotation;

            if (setLocalRotation)
            {
                pooledObject.transform.localRotation = Quaternion.identity;
                pooledObject.transform.localPosition = new Vector3(0, pooledObject.transform.localPosition.y, 0);
            }

            // Set the GameObject to active
            pooledObject.SetActive(true);
            return pooledObject;
        }

        /// <summary>
        /// Recycle() takes a GameObject that's currently being used and puts it back into the shared pool.
        /// </summary>
        public void Recycle(GameObject pooledObject)
        {
            Debug.Assert(m_usedList.Contains(pooledObject));

            // Put the GameObject back in the free list, reparent to its pooler, and disable it
            m_usedList.Remove(pooledObject);
            m_freeList.Add(pooledObject);
            pooledObject.SetActive(false);
        }

        /// <summary>
        /// RecycleAll() puts all the used objects back into the shared pool.
        /// </summary>
        public void RecycleAll()
        {
            if (m_usedList.Count == 0)
                return;
            
            for(int i=m_usedList.Count-1; i >=0; i--)
                Recycle(m_usedList[i]);
        }

        /// <summary>
        /// Shuffle() will randomize the order of objects in the free list.
        /// </summary>
        public void Shuffle()
            => m_freeList.Shuffle();

        void OnDestroy()
            => ClearAll();

        void ClearAll()
        {
            for(int i=m_usedList.Count-1; i >=0; i--)
            {
                if (Application.isPlaying)
                    Destroy(m_usedList[i]);
                else
                    DestroyImmediate(m_usedList[i]);
            }
            
            m_usedList.Clear();

            for(int i=m_freeList.Count-1; i >=0; i--)
            {
                if (Application.isPlaying)
                    Destroy(m_freeList[i]);
                else
                    DestroyImmediate(m_freeList[i]);
            }
            
            m_freeList.Clear();
        }
    }
}
