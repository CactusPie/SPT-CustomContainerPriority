using System.Collections.Generic;

namespace CactusPie.CustomContainerPriority.Data
{
    internal sealed class ArrayPriority
    {
        public IEnumerable<GClass2318> Array { get; }

        public int Priority { get; }

        public ArrayPriority(IEnumerable<GClass2318> array, int priority)
        {
            Array = array;
            Priority = priority;
        }
    }
}