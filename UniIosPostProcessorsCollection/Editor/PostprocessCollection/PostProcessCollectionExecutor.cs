using System.IO;
#if UNITY_IOS && UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

namespace PostprocessCollection
{
    /// <summary>
    /// Main class of the PostprocessCollection.
    /// Hold the setting scriptable object and
    /// starts all the other postprocess actions.
    /// </summary>
    public class PostProcessCollectionExecutor : ScriptableObject
    {
        public CustomXcodeProjectModifications XcodeModificationsScriptableObject;
#if UNITY_IOS && UNITY_EDITOR
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            var dummy = CreateInstance<PostProcessCollectionExecutor>();
            if (dummy.XcodeModificationsScriptableObject == null)
                return;

            var xcodeModifications = dummy.XcodeModificationsScriptableObject;

            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            var target_name = PBXProject.GetUnityTargetName();
            var target = project.TargetGuidByName(target_name);
            
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            

            AddFrameworksPostprocess.AddFrameworks(project, target, xcodeModifications.Frameworks);
            ProjectPropertyPostprocess.AddProperty(project, target, xcodeModifications.Flags);
            AddPropertiesPostprocess.AddProperties(plist, xcodeModifications.PlistKeys);
            CopyFilesPostprocess.CopyAllFilesFromDirectory(xcodeModifications.CopyFilesDirectory, path, project, target);
            AddEntitlementsPostprocess.AddEntitlements(xcodeModifications.EntitlementsFile, project, path, target, target_name);
            ReplaceDelegatePostprocess.ReplaceDelegate(xcodeModifications.NewDelegateFile, path);
            
            project.WriteToFile(projectPath);
            File.WriteAllText(projectPath, project.WriteToString());
            File.WriteAllText(plistPath, plist.WriteToString());

            DestroyImmediate(dummy);
        }
#endif
    }
}
