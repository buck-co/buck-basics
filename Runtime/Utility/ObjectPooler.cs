using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    [System.Serializable]
    public struct PooledObject
    {
        [SerializeField] public GameObject m_prefab;
        [SerializeField] public int m_numberOfObjects;
    }

    public class ObjectPooler : MonoBehaviour
    {
        List<GameObject> m_freeList = new List<GameObject>();
        public List<GameObject> FreeList { get => m_freeList;  }
        List<GameObject> m_usedList = new List<GameObject>();
        public List<GameObject> UsedList { get => m_usedList; }

        /// <summary>
        /// GenerateObjects() takes a list of PooledObject structs and instantiates GameObjects for use in the shared pool.
        /// Each PooledObject struct contains the GameObject that should be instantiated and an int that represents the number of those GameObjects that should be available in the shared pool.
        /// </summary>
        public void GenerateObjects(List<PooledObject> pooledObjects, bool setParent = true)
        {
			// Destroy all of the old GameObjects
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
            List<PooledObject> pooledObjects = new List<PooledObject>();
            pooledObjects.Add(pooledObject);
            GenerateObjects(pooledObjects, setParent);
        }

        /// <summary>
        /// Retrieve() returns a GameObject from the shared pool at the desired position and rotation.
        /// If there's more than one type of GameObject in the shared pool, it will choose randomly.
        /// </summary>
        public GameObject Retrieve(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), bool setLocalRotation = false)
        {
            int numFree = m_freeList.Count;
            if (numFree == 0)
            {
                Debug.LogWarning("Object pool is out of objects!");
                return null;
            }

            // Pull a GameObject from the end of the free list
            GameObject pooledObject = m_freeList[numFree - 1];
            m_freeList.RemoveAt(numFree - 1);
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
        /// RecycleAll() puts all of the used objects back into the shared pool.
        /// </summary>
        public void RecycleAll()
        {
            if (m_usedList.Count == 0) return;
            for(int i=m_usedList.Count-1; i >=0; i--)
                Recycle(m_usedList[i]);
        }

        /// <summary>
        /// Shuffle() will randomize the order of objects in the free list.
        /// </summary>
        public void Shuffle()
        {
            m_freeList.Shuffle();
        }

        private void ClearAll()
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
