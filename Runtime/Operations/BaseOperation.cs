using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    public static class OperationExtensionMethods
    {        
        /// <summary>
        /// Loops through a ICollection of Operations and executes them all
        /// </summary>
        public static void Execute(this ICollection<BaseOperation> operations)
        {
            if (operations == null)
                return;
            foreach(BaseOperation o in operations)
                o.Execute();
        }
    }

    public abstract class BaseOperation : ISerializationCallbackReceiver
    {
        public abstract bool Serialized { get; set; }
        public abstract void Execute();
        public abstract void OnBeforeSerialize();
        public abstract void OnAfterDeserialize();
    }
}
