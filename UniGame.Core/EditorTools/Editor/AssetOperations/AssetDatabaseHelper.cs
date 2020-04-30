namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class AssetDatabaseHelper
    {
        public static IList<T> GetAllAssetsFromPath<T>(string path) where T : Object
        {
            var directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles().Select(x => x.FullName.ToPathFromAssets());

            return files.Select(AssetDatabase.LoadAssetAtPath<T>).OfType<T>().ToList();
        }

        public static bool TryGetAsset<T>(string path, out T data) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) {
                data = asset;
                return true;
            }

            data = null;
            return false;
        }

        public static bool AssetExists<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) {
                return true;
            }

            return false;
        }
    }
}