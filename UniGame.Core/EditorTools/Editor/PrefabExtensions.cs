namespace UniModules.UniGame.Core.EditorTools.Editor
{
    using AssetOperations;
    using UnityEditor;
    using UnityEngine;

    public static class PrefabExtensions
    {
        public static bool TryApplyPrefab(this GameObject source, string path, out GameObject prefab)
        {
            if (PrefabUtility.IsPartOfAnyPrefab(source) && AssetDatabaseHelper.TryGetAsset<GameObject>(path, out var result)) {
                PrefabUtility.ApplyPrefabInstance(source, InteractionMode.AutomatedAction);
                prefab = result;
                return true;
            }

            prefab = null;
            return false;
        }

        public static bool TryApplyPrefab(this GameObject source)
        {
            if (PrefabUtility.IsPartOfAnyPrefab(source)) {
                PrefabUtility.ApplyPrefabInstance(source, InteractionMode.AutomatedAction);
                return true;
            }

            return false;
        }
    }
}