
using System.Collections.Generic;

namespace Buck
{
    
    public static class OperationExtensionMethods
    {        
        /// <summary>
        /// Loops through a ICollection of Operations and executes them all
        /// </summary>
        static public void Execute(this ICollection<BaseOperation> operations)
        {
            if (operations == null)
                return;

          
            foreach(BaseOperation o in operations)
            {
                o.Execute();
            }
        }
            
    }

    public abstract class BaseOperation
    {
        public abstract void Execute();
    }
}
