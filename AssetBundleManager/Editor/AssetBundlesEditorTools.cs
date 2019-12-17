namespace UniGreenModules.AssetBundleManager.Editor
{
    public static class AssetBundlesEditorTools {
        public static string GetBundleName(string path) {
            if (string.IsNullOrEmpty(path) == true) return string.Empty;
            var abName = UnityEditor.AssetDatabase.GetImplicitAssetBundleName(path);
            return string.IsNullOrEmpty(abName) ? string.Empty : abName;
        }
    }
}

