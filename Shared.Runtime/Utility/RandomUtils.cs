namespace UniGame.Shared.Runtime.Utility
{
    using UnityEngine;

    public static class RandomUtils
    {
        public static float Random(this Vector2 value)
        {
            return UnityEngine.Random.Range(value.x, value.y);
        }
        
        public static int SelectNextIndex(int count, int previous,bool selectRandomly)
        {
            if (count <= 0) return -1;
            if (!selectRandomly)
                return SelectNextIndex(count, previous);
            
            var index = UnityEngine.Random.Range(0, count);
            return index;
        }

        public static int SelectNextIndex(int count,int previous)
        {
            if (count <= 0) return -1;
            var next = previous + 1;
            next = next >= count ? 0 : next;
            return next;
        }
    }
}
