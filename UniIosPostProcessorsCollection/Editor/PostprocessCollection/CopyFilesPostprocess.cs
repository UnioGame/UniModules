#if UNITY_IOS
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace PostprocessCollection
{
    /// <summary>
    /// List through all the files in the directory, get path to these files
    /// and add them to 'Copy resources' inside XCode project
    /// </summary>
    public static class CopyFilesPostprocess
    {
        public static void CopyAllFilesFromDirectory(DefaultAsset defaultAsset, string buildPath,
                                                    PBXProject project, string target)
        {
            if (defaultAsset == null) return;
            var files = Directory.GetFiles(AssetDatabase.GetAssetPath(defaultAsset));
            var destinationPath = buildPath + "/";
            foreach (var file in files)
            {
                var nameAndExtension = file.Split('/').LastOrDefault();
                if(nameAndExtension != null && nameAndExtension.ToLower().Contains(".meta"))
                    continue;
                var assetLocation = AssetDatabase.GetAssetPath(defaultAsset) + "/" + nameAndExtension;
                var assetDestination = destinationPath + nameAndExtension;
                FileUtil.CopyFileOrDirectory(assetLocation, assetDestination);
                var grGUID = project.AddFolderReference(destinationPath + nameAndExtension, nameAndExtension);
                project.AddFileToBuild(target, grGUID);
            }
        }
    }
}
#endif