using UnityEngine;
using System;

namespace Buck
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector] byte[] m_guidByteArray;
        public Guid Guid => new Guid(m_guidByteArray);
        public byte[] GuidByteArray => m_guidByteArray;

        public static T FindByGuid<T>(Guid guid, string path) where T : BaseScriptableObject
        {
            UnityEngine.Object[] dataFromDisk = Resources.LoadAll(path, typeof(T));
            foreach (T obj in dataFromDisk)
                if (obj.GetType() == typeof(T) && guid == obj.Guid)
                    return obj;

            return null;
        }

        public static T FindByGuid<T>(byte[] m_guidByteArray, string path) where T : BaseScriptableObject
        {
            return FindByGuid<T>(new Guid(m_guidByteArray), path);
        }

        public void OnValidate()
        {
            #if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            m_guidByteArray = new Guid(UnityEditor.AssetDatabase.AssetPathToGUID(path)).ToByteArray();
            #endif
        }
    }
}