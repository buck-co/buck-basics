using UnityEngine;
using System;

namespace Buck
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector] byte[] m_guidByteArray;
        public Guid Guid => new Guid(m_guidByteArray);
        public byte[] GuidByteArray => m_guidByteArray;

        void OnValidate()
        {
            #if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            m_guidByteArray = new Guid(UnityEditor.AssetDatabase.AssetPathToGUID(path)).ToByteArray();
            #endif
        }
    }
}