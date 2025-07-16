// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using System;

namespace Buck
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector] byte[] m_guidByteArray;
        public Guid Guid => new(m_guidByteArray);
        public byte[] GuidByteArray => m_guidByteArray;

        public static List<T> FindAll<T>(string path) where T : BaseScriptableObject
        {
            List<T> objs = new List<T>();

            UnityEngine.Object[] objectsFromDisk = Resources.LoadAll(path, typeof(T));
            foreach (T obj in objectsFromDisk)
                if (obj.GetType() == typeof(T))
                    objs.Add(obj);

            return objs;
        }

        public static T FindByGuid<T>(Guid guid, string path) where T : BaseScriptableObject
        {
            UnityEngine.Object[] objectsFromDisk = Resources.LoadAll(path, typeof(T));
            foreach (T obj in objectsFromDisk)
                if (obj.GetType() == typeof(T) && guid == obj.Guid)
                    return obj;

            return null;
        }

        public static T FindByGuid<T>(byte[] m_guidByteArray, string path) where T : BaseScriptableObject
            => FindByGuid<T>(new Guid(m_guidByteArray), path);

        public void OnValidate()
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            m_guidByteArray = new Guid(UnityEditor.AssetDatabase.AssetPathToGUID(path)).ToByteArray();
#endif
        }
    }
}
