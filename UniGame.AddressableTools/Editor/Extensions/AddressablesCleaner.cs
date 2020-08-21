namespace UniModules.UniGame.AddressableTools.Editor.Extensions
{
    using System;
    using System.IO;
    using Core.EditorTools.Editor.Tools;
    using UniCore.Runtime.ProfilerTools;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Build;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    public static class AddressablesCleaner
    {
        public const string AddressablesCachePath     = "./Library/com.unity.addressables";
        public const string StreamingAddressablesPath = "/aa";
            
        [MenuItem("UniGame/Addressables/Clean Library Cache")]
        public static void RemoveLibraryCache()
        {
            try {
                EditorFileUtils.DeleteDirectoryFiles(AddressablesCachePath);
                EditorFileUtils.DeleteSubDirectories(AddressablesCachePath);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

            GameLog.Log("Addressables Library Cache Removed");
        }

        [MenuItem("UniGame/Addressables/Clean Default Context Builder")]
        public static void CleanDefaultContextBuilder()
        {
            AddressableAssetSettings.CleanPlayerContent(null);
        }
 
        [MenuItem("UniGame/Addressables/Clean All")]
        public static void CleanAll()
        {
            RemoveLibraryCache();
            RemoveStreamingCache();
            CleanPlayerContent(null);
            GameLog.Log("Addressables Cache Removed");
        }
        
        public static void CleanPlayerContent(IDataBuilder builder)
        {
            AddressableAssetSettings.CleanPlayerContent(builder);
        }

        
        public static void RemoveStreamingCache()
        {
            try {
                var targetPath = Application.streamingAssetsPath + StreamingAddressablesPath;
                EditorFileUtils.DeleteDirectory(targetPath);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

        }


    }
}
