namespace UniGreenModules.AssetBundleManager.Editor
{
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using UnityEditor;
	using UnityEngine;

	public class BuildScript
	{
		public static string overloadedDevelopmentServerURL = "";
	
		public static void BuildAssetBundles()
		{
			// Choose the output path according to the build target.
			string outputPath = AssetBundlesEditorCommands.BundlesStreamingPath;
			if (!Directory.Exists(outputPath) )
				Directory.CreateDirectory (outputPath);
	
			//@TODO: use append hash... (Make sure pipeline works correctly with it.)
			BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		}
	
		public static void WriteServerURL()
		{
			string downloadURL;
			if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
			{
				downloadURL = overloadedDevelopmentServerURL;
			}
			else
			{
				IPHostEntry host;
				string localIP = "";
				host = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress ip in host.AddressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						localIP = ip.ToString();
						break;
					}
				}
				downloadURL = "http://"+localIP+":7888/";
			}
			
			string assetBundleManagerResourcesDirectory = "Assets/AssetBundleManager/_resources";
			string assetBundleUrlPath = Path.Combine (assetBundleManagerResourcesDirectory, "AssetBundleServerURL.bytes");
			Directory.CreateDirectory(assetBundleManagerResourcesDirectory);
			File.WriteAllText(assetBundleUrlPath, downloadURL);
			AssetDatabase.Refresh();
		}
	
		public static void BuildPlayer()
		{
			var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built GameData", "", "");
			if (outputPath.Length == 0)
				return;
	
			string[] levels = GetLevelsFromBuildSettings();
			if (levels.Length == 0)
			{
				Debug.Log("Nothing to build.");
				return;
			}
	
			string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
			if (targetName == null)
				return;
	
			// Build and copy AssetBundles.
			BuildScript.BuildAssetBundles();
			WriteServerURL();
	
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
		}
		
		public static void BuildStandalonePlayer()
		{
			var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built GameData", "", "");
			if (outputPath.Length == 0)
				return;
			
			string[] levels = GetLevelsFromBuildSettings();
			if (levels.Length == 0)
			{
				Debug.Log("Nothing to build.");
				return;
			}
			
			string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
			if (targetName == null)
				return;
			
			// Build and copy AssetBundles.
			BuildScript.BuildAssetBundles();
			AssetDatabase.Refresh();
			
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
		}
	
		public static string GetBuildTargetName(BuildTarget target)
		{
			switch(target)
			{
			case BuildTarget.Android :
				return "/test.apk";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "/test.exe";
			case BuildTarget.StandaloneOSX:
				return "/test.app";
			case BuildTarget.WebGL:
				return "";
				// Add more build targets for your own.
			default:
				Debug.Log("Target not implemented.");
				return null;
			}
		}
	
		static void CopyAssetBundlesTo(string outputPath)
		{
			// Clear streaming assets folder.
			FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
			Directory.CreateDirectory(outputPath);

			// Setup the source folder for assetbundles.
		    var source = AssetBundlesEditorCommands.BundlesStreamingPath;
            if (!System.IO.Directory.Exists(source) )
				Debug.Log("No assetBundle output folder, try to build the assetBundles first.");
		}
	
		static string[] GetLevelsFromBuildSettings()
		{
			List<string> levels = new List<string>();
			for(int i = 0 ; i < EditorBuildSettings.scenes.Length; ++i)
			{
				if (EditorBuildSettings.scenes[i].enabled)
					levels.Add(EditorBuildSettings.scenes[i].path);
			}
	
			return levels.ToArray();
		}
	}
}