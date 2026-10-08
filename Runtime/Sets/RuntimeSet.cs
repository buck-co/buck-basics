// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;

namespace Buck
{
    public abstract class RuntimeSet<T> : GameEvent
    {
        public List<T> Items = new();

        // The items are objects of the Play session that added them; see GameEvent.
        internal override void OnPlaySessionStarted()
            => Items.Clear();

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