namespace UniModules.UniGame.AddressableTools.Editor.AddressablesChecker
{
    using System.Collections.Generic;
    using UniCore.Runtime.ProfilerTools;
    using UnityEditor;
    using UnityEditor.AddressableAssets;

    public static class AddressablesChecker
    {
        [MenuItem("UniGame/Addressables/Check")]
        private static void Check()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            
            var uniqueGuid = new HashSet<string>();
            var uniquePaths = new HashSet<string>();
            
            foreach (var assetGroup in settings.groups)
            {
                foreach (var assetEntry in assetGroup.entries)
                {
                    if (!uniqueGuid.Add(assetEntry.guid))
                    {
                        GameLog.LogError($"Found a duplicate: [{assetEntry.AssetPath}] ({assetEntry.guid})");
                    }

                    var assetPath = AssetDatabase.GUIDToAssetPath(assetEntry.guid);
                    if (!uniquePaths.Add(assetEntry.AssetPath) || !assetEntry.AssetPath.Equals(assetPath))
                    {
                        GameLog.LogError($"Found an invalid path: [{assetEntry.AssetPath}] ({assetEntry.guid})");
                    }
                }
            }
        }
    }
}