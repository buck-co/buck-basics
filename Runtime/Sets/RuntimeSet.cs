using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    public abstract class RuntimeSet<T> : BaseScriptableObject
    {
        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }
    }
}