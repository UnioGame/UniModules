namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations
{
    using UnityEditor;
    using UnityEngine;

    public static class AssetDatabaseHelper
    {
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