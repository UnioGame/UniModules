using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

namespace PostprocessCollection
{

    /// <summary>
    /// Adds properties to XCode project.
    /// For example it's common case to add
    /// a Linker Flag -Objc in OTHER_LDFLAGS to project.
    /// Also Facebook plugin may want you to add -lxml2 flag to OTHER_LDFLAGS.
    /// Both actions can be done using this post process.
    /// </summary>
    public static class ProjectPropertyPostprocess //: MonoBehaviour
    {
//    [PostProcessBuild(700)]
//    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
//    {
//        if (target != BuildTarget.iOS)
//        {
//            return;
//        }
// 
//        string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
//        PBXProject proj = new PBXProject();
//        proj.ReadFromString(File.ReadAllText(projPath));
//        string targetGUID = proj.TargetGuidByName("Unity-iPhone");
//        proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-lxml2"); 
//        File.WriteAllText(projPath, proj.WriteToString());
//    }

        public static void AddProperty(PBXProject proj, string targetGUID, List<BuildProperties> properties)
        {
            if(properties == null) return;
            foreach (var property in properties)
            {
                proj.AddBuildProperty(targetGUID, property.Name, property.Value);
            }
        }
    }
}