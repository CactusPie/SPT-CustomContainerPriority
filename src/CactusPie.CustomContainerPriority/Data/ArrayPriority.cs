namespace CactusPie.CustomContainerPriority.Data
{
    internal sealed class ArrayPriority
    {
        public GClass2318[] Array { get; }

        public int Priority { get; }

        public ArrayPriority(GClass2318[] array, int priority)
        {
            Array = array;
            Priority = priority;
        }
    }
}