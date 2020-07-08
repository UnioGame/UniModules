using System.IO;
using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.EditorTools.Editor.Tools
{
    public static class EditorFilesUtils
    {

        public static void DeleteDirectoryFiles(string path)
        {
            if (!Directory.Exists(path))
                return;
            foreach (var file in new DirectoryInfo(path).GetFiles()) {
                file.Delete();
            }
        }
    
        public static void DeleteSubDirectories(string path)
        {
            if (!Directory.Exists(path))
                return;
            foreach (var subDir in new DirectoryInfo(path).GetDirectories()) {
                subDir.Delete(true);
            }
        }
        
        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path)) {
                GameLog.Log($"REMOVE Folder {path}");
                Directory.Delete(path,true);
                return;
            }
        
            GameLog.Log($"Addressables:  FAILED Remove addressable folder {path} NOT FOUND");
        }
    }
}
