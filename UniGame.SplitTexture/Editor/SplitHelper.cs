namespace UniModules.UniGame.SplitTexture.Editor
{
    using UnityEngine;

    public static class SplitHelper
    {
        public static int GetSplitCount(float size, int maxSize)
        {
            return Mathf.CeilToInt(size / maxSize);
        }

        public static string GetSplittedTextureName(int index, string prefix)
        {
            return $"{prefix}_part_{index}";
        }
    }
}