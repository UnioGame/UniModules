namespace UniGame.Utils.Editor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEditor;

    public static class FindReferencesInProject
    {
        private const string MenuItem = "Assets/Find References In Project";

        [MenuItem(MenuItem, false, 25)]
        private static void FindReferences()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var referenceCache = new Dictionary<string, List<string>>();
            var guids          = AssetDatabase.FindAssets("");
            foreach (var guid in guids)
            {
                var assetPath    = AssetDatabase.GUIDToAssetPath(guid);
                var dependencies = AssetDatabase.GetDependencies(assetPath, false);

                foreach (var dependency in dependencies)
                {
                    if (referenceCache.ContainsKey(dependency))
                    {
                        if (!referenceCache[dependency].Contains(assetPath))
                            referenceCache[dependency].Add(assetPath);
                    }
                    else
                    {
                        referenceCache[dependency] = new List<string>{assetPath};
                    }
                }
            }
            
            UnityEngine.Debug.Log($"Build index takes {stopWatch.ElapsedMilliseconds} milliseconds");

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            UnityEngine.Debug.Log($"Find: {path}", Selection.activeObject);

            if (referenceCache.ContainsKey(path))
            {
                foreach (var reference in referenceCache[path])
                {
                    UnityEngine.Debug.Log(reference, AssetDatabase.LoadMainAssetAtPath(reference));
                }
            }
            else
            {
                UnityEngine.Debug.Log("No references");
            }
            
            referenceCache.Clear();
        }
    }
}