using System.Collections.Generic;

namespace Buck
{
    public abstract class RuntimeSet<T> : GameEvent
    {
        public List<T> Items = new();

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