namespace UniModules.UniGame.Core.EditorTools.Editor
{
    using AssetOperations;
    using UnityEngine;

    public static class PrefabExtensions
    {
        public static bool TryApplyPrefab(this GameObject source, string path, out GameObject prefab)
        {
#if UNITY_EDITOR
            if (source && UnityEditor.PrefabUtility.IsPartOfAnyPrefab(source) && AssetDatabaseHelper.TryGetAsset<GameObject>(path, out var result)) {
                UnityEditor.PrefabUtility.ApplyPrefabInstance(source, UnityEditor.InteractionMode.AutomatedAction);
                prefab = result;
                return true;
            }
#endif
            prefab = null;
            return false;
        }

        public static bool TryApplyPrefab(this GameObject source)
        {
            if (!source)
                return false;
#if UNITY_EDITOR
            if (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(source)) {
                UnityEditor.PrefabUtility.ApplyPrefabInstance(source, UnityEditor.InteractionMode.AutomatedAction);
                return true;
            }
#endif
            return false;
        }
    }
}