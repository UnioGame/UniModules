using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace PostprocessCollection
{
    /// <summary>
    /// Takes given Entitlements file, copies it to the
    /// Xcode project folder and ads a reference to it
    /// in the project settings
    /// </summary>
    public static class AddEntitlementsPostprocess
    {
        public static void AddEntitlements(DefaultAsset file, PBXProject project, string path, string target, string target_name)
        {
            if (file == null) return;
            
            var src = AssetDatabase.GetAssetPath(file);
            var file_name = Path.GetFileName(src);
            var dst = path + "/" + target_name + "/" + file_name;
            FileUtil.CopyFileOrDirectory(src, dst);
            project.AddFile(target_name + "/" + file_name, file_name);
            project.SetBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);
        }
    }
}