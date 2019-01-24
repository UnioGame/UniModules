using System.IO;
using System.Linq;
using Assets.UI.Windows.Tools.Editor;
using UniModule.UnityTools.AssetBundleManager;
using UniModule.UnityTools.ProfilerTools;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesModule {

    public class AssetBundlesEditorCommands
    {
        private const string _simulationMode = "Tools/AssetBundles/Bundles Simulate Mode";
        private const string _lazyMode = "Tools/AssetBundles/Bundles Lazy Mode";
        private const string _removeAllBundlesTags = "Tools/AssetBundles/Remove Unused Asset Bundle Names";
        private const string _removeUnusedAssetBundleNames = "Tools/AssetBundles/Remove Unused AssetBundle Names";
        private const string _removeFolderBundlesTags = "Assets/Remove Folder Bundles Tags";

        private static string _streamingPath;

        public static string BundlesStreamingPath
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingPath))
                {
                    _streamingPath = string.Format("{0}/{1}/", Application.streamingAssetsPath, "AssetBundles");
                }

                return _streamingPath;

            }
        }

        [MenuItem(_simulationMode)]
        public static void ToggleSimulationMode()
        {
            AssetBundleConfiguration.IsSimulateMode = !AssetBundleConfiguration.IsSimulateMode;
        }

        [MenuItem(_removeUnusedAssetBundleNames)]
        public static void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

        }

        [MenuItem(_removeFolderBundlesTags)]
        public static void RemoveBundlesTagsFromFolder()
        {
            global::AssetBundlesEditorOperations.RemoveBundlesTagsFromFolder();
        }

        [MenuItem(_removeAllBundlesTags)]
        public static void RemoveAllBundlesTags()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        [MenuItem(_simulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(_simulationMode, AssetBundleConfiguration.IsSimulateMode);
            return true;
        }

        [MenuItem(_lazyMode)]
        public static void ToggleLazyMode()
        {
            AssetBundleConfiguration.IsLazyMode = !AssetBundleConfiguration.IsLazyMode;
        }

        [MenuItem(_lazyMode, false)]
        public static bool ToggleLazyModeValidate()
        {
            Menu.SetChecked(_lazyMode, AssetBundleConfiguration.IsLazyMode);
            return true;
        }

        [MenuItem("Tools/AssetBundles/Build/Windows")]
        public static void BuildWindowsAllAssetBundles()
        {
            var abTargetPath = BundlesStreamingPath;
            CreateBundlesFolders(abTargetPath);
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, BuildTarget.StandaloneWindows);
        }

        [MenuItem("Tools/AssetBundles/Build/StandaloneOSX")]
        public static void BuildAllAssetBundlesStandaloneOSX()
        {
            var abTargetPath = BundlesStreamingPath;
            CreateBundlesFolders(abTargetPath);
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, BuildTarget.StandaloneOSX);
        }

        [MenuItem("Tools/AssetBundles/Build/Compressed")]
        public static void BuildAssetBundlesCompressed()
        {

            var abTargetPath = PrepareToBuild();
            AssetBundleConfiguration.IsSimulateMode = true;
            var buildOptions = BuildAssetBundleOptions.None;
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, EditorUserBuildSettings.activeBuildTarget);

        }

        [MenuItem("Tools/AssetBundles/Build/ChunkBasedCompression")]
        public static void BuildAssetBundlesCurrentChunkBasedCompression()
        {
            var abTargetPath = PrepareToBuild();
            AssetBundleConfiguration.IsSimulateMode = true;
            var buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, EditorUserBuildSettings.activeBuildTarget);
        }

        public static void BuildAssetBundlesWithConfiguration(BuildTarget platform,bool cleanUp = true)
        {
            var abTargetPath = PrepareToBuild(cleanUp);

            var configuration = AssetEditorTools.GetAssets<AssetBundleBuildConfiguration>().FirstOrDefault();

            var buildOptions = configuration == null
                ? BuildAssetBundleOptions.ChunkBasedCompression
                : configuration.GetRuntimeBundleOptions(platform);

            Debug.LogFormat("Build bundles with configuration file with options [{0}]", buildOptions);

            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, platform);
        }

        [MenuItem("Tools/AssetBundles/Build/Configuration")]
        public static void BuildAssetBundlesWithConfiguration()
        {

            var platform = EditorUserBuildSettings.activeBuildTarget;

            BuildAssetBundlesWithConfiguration(platform);

        }

        [MenuItem("Tools/AssetBundles/Build/Configuration - No cleanup")]
        public static void BuildAssetBundlesConfigurationNoCleanup()
        {

            var platform = EditorUserBuildSettings.activeBuildTarget;

            BuildAssetBundlesWithConfiguration(platform,false);

        }

        [MenuItem("Tools/AssetBundles/Build/Current Uncompressed")]
        public static void BuildAssetBundlesCurrentUncompressed()
        {

            var path = PrepareToBuild();
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
            Debug.LogFormat("Build bundles with configuration file with options [{0}]", buildOptions);
            BuildPipeline.BuildAssetBundles(path, buildOptions, EditorUserBuildSettings.activeBuildTarget);

        }

        public static string PrepareToBuild(bool cleanUp = true)
        {
            var abTargetPath = BundlesStreamingPath;
            AssetBundleConfiguration.IsSimulateMode = true;
            CreateBundleFolder(abTargetPath);
            if(cleanUp)
                CleanUpBundlesFolder(abTargetPath);
            return abTargetPath;
        }

        [MenuItem("Tools/AssetBundles/Build/Dry Build(Current)")]
        public static void DryAssetBundlesCurrent()
        {
            var abTargetPath = PrepareToBuild();
            var buildOptions = BuildAssetBundleOptions.DryRunBuild;
            var id = GameProfiler.BeginWatch("Dry bundles build");
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, EditorUserBuildSettings.activeBuildTarget);
            GameProfiler.StopWatch(id);
        }

        [MenuItem("Tools/AssetBundles/Get AssetResource names")]
        public static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names) Debug.Log("AssetResource: " + name);
        }

        [MenuItem("Tools/AssetBundles/Build/Android")]
        public static void BuildAndroidAllAssetBundles()
        {
            var abTargetPath = BundlesStreamingPath;
            CreateBundlesFolders(abTargetPath);
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;
            BuildPipeline.BuildAssetBundles(abTargetPath, buildOptions, BuildTarget.Android);
        }

        private static void CreateAssetBundleFolder(string path)
        {
            string outputPath = path;
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
        }

        private static void CreateBundlesFolders(string targetLocation)
        {
            var abTargetPath = targetLocation;
            CreateAssetBundleFolder(abTargetPath);
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
            {
                if (!name.Contains("/")) continue;
                var items = name.Split('/');
                for (int i = 0; i < items.Length - 1; i++)
                {
                    var path = Path.Combine(abTargetPath, items[i]);
                    CreateAssetBundleFolder(path);
                }
            }
        }

        private static void CleanUpBundlesFolder(string targetLocation)
        {
            
            var files = Directory.GetFiles(targetLocation);
            foreach (var file in files) {
                File.Delete(file);
            }

        }

        private static void CreateBundleFolder(string targetLocation)
        {
            if (Directory.Exists(targetLocation) == false) {
                CreateBundlesFolders(targetLocation);
                return;
            }
        }
    }

}