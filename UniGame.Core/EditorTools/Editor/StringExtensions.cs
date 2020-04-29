namespace UniModules.UniGame.Core.EditorTools.Editor
{
    using System;

    public static class StringExtensions
    {
        public static string ToPathFromAssets(this string path)
        {
            if (path == null)
                return string.Empty;
            
            var index = path.IndexOf("Assets", StringComparison.Ordinal);
            if(index >= 0) {
                path = path.Substring(index);
            }
            
            return path;
        }
    }
}