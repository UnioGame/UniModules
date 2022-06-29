namespace UniGame.Utils.Runtime
{
    using System;

    public static class ArrayUtilities
    {
        public static void Insert<T>(ref T[] target, T value, int index)
        {
            if (index < target.GetLowerBound(0) || 
                index > target.GetUpperBound(0) + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, 
                    "Array index out of bounds.");
            }
            
            Array.Resize(ref target, target.Length + 1);
            
            Array.Copy(target, index, target, index + 1, 
                target.Length - index - 1);

            target.SetValue(value, index);
        }

        public static void RemoveAt<T>(ref T[] target, int index)
        {
            if (index < target.GetLowerBound(0) || 
                index > target.GetUpperBound(0))
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, 
                    "Array index out of bounds.");
            }

            if (index < target.GetUpperBound(0))
            {
                Array.Copy(target, index + 1, target, index, 
                    target.Length - index - 1);
            }

            Array.Resize(ref target, target.Length - 1);
        }
    }
}