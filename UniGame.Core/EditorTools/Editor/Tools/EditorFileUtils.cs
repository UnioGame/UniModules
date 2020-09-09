using System.IO;
using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.EditorTools.Editor.Tools
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class EditorFileUtils
    {
        private static string assetsFolderName       = "Assets";
        private static char moveDirectorySeparator = '/';
        
        public static bool WriteAssetsContent(string targetPath, string content)
        {
            if (string.IsNullOrEmpty(targetPath))
                return false;

            var result = ReadContent(targetPath, false);
            var path = result.path;
            var activeContent = result.content;

            if (string.Equals(activeContent, content)) {
                return false;
            }

            File.WriteAllText(path,content);

            Debug.Log($"WRITE CONTENT TO {path}");

            AssetDatabase.Refresh();

            return true;
        }

        public static (string path,string content) ReadContent(string targetPath, bool createIfNotExists = false)
        {
            if (string.IsNullOrEmpty(targetPath))
                return (string.Empty,string.Empty);

            if (targetPath.StartsWith(assetsFolderName, StringComparison.OrdinalIgnoreCase)) {
                targetPath = targetPath.Remove(0, assetsFolderName.Length);
            }

            var path = Combine(Application.dataPath, targetPath);

            ValidateDirectories(path);

            var fileExists = File.Exists(path);
            if (!fileExists && createIfNotExists) {
                File.WriteAllText(path,string.Empty);
                AssetDatabase.Refresh();
                return (path,string.Empty);
            }

            var activeContent = fileExists ?
                File.ReadAllText(path) :
                string.Empty;

            return (path,activeContent);
        }

        public static string ToProjectPathWithoutExtension(this string globalPath)
        {
            var assetPath = globalPath.Replace(Application.dataPath, string.Empty);
            assetPath = Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath));
            assetPath = Combine(assetsFolderName, assetPath);
            return assetPath;
        }
        
        public static string ToProjectPath(this string globalPath)
        {
            var assetPath = globalPath.Replace(Application.dataPath, string.Empty);
            assetPath = Combine(assetsFolderName, assetPath);
            return assetPath;
        }

        /// <summary>
        /// Combines two paths, and replaces all backslases with forward slash.
        /// </summary>
        public static string Combine(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
                return b;
            a = a.FixUnityPath().TrimEndPath();
            b = b.FixUnityPath().TrimStartPath();
            return a + Path.DirectorySeparatorChar + b;
        }

        public static string FixDirectoryPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return path.TrimEndPath() + Path.DirectorySeparatorChar;
        }
        
        public static string CombinePath(this string a, string b) => Combine(a, b);

        public static string TrimStartPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return path.TrimStart('/').TrimStart('\\');
        }
        
        public static string TrimEndPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return path.TrimEnd('/').TrimEnd('\\');
        }
        
        public static string FixUnityPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            switch (Path.DirectorySeparatorChar) {
                case '\\':
                    return path.Replace("/", "\\").TrimEnd('\\');
                case '/':
                    return path.Replace("\\", "/").TrimEnd('/');
            }

            return path.Replace("\\", "/").TrimEnd('/');
        }

        /// <summary>
        /// move dir from source location to dest
        /// dest location will be removed by default
        /// </summary>
        /// <param name="source">source dir path</param>
        /// <param name="dest">dest dir path</param>
        /// <param name="removeDest">is directory should be removed before copy? TRUE by default</param>
        /// <returns></returns>
        public static bool MoveDirectory(string source, string dest, bool removeDest = true)
        {
            if(string.IsNullOrEmpty(source) || Directory.Exists(source) == false)
                return false;
            if (string.IsNullOrEmpty(dest))
                return false;

            if (source.Equals(dest, StringComparison.OrdinalIgnoreCase))
                return true;

            dest = dest.FixDirectoryPath();
            
            if (removeDest) {
                DeleteDirectory(dest);
            }

            try {
                //hack for https://docs.unity3d.com/ScriptReference/FileUtil.MoveFileOrDirectory.html
                source = source.Replace('\\', moveDirectorySeparator);
                dest   = dest.Replace('\\', moveDirectorySeparator);
            
                FileUtil.MoveFileOrDirectory(source,dest);

                return true;
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return false;
        }
        

        public static string[] SplitPath(this string path)
        {
            path = FixUnityPath(path);
            return path.Split(Path.DirectorySeparatorChar).ToArray();
        }

        public static void ValidateDirectories(string sourcePath)
        {
            var directoryPath = Path.GetDirectoryName(sourcePath);
            var directories = directoryPath.FixUnityPath();
            var path = string.Empty;
            
            if (string.IsNullOrEmpty(directoryPath) == false) {
                var folders = SplitPath(directories);

                foreach (var folder in folders) {
                    if (string.IsNullOrEmpty(path)) {
                        path = folder;
                        continue;
                    }
                    path = Combine(path,folder);
                    if (Directory.Exists(path)) {
                        continue;
                    }

                    Debug.Log(path);
                    Directory.CreateDirectory(path);
                }
            }
        }
        
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
        
        public static void DeleteDirectory(string path,bool recursive = true)
        {
            if (Directory.Exists(path)) {
                GameLog.Log($"REMOVE Folder {path}");
                Directory.Delete(path,recursive);
                return;
            }
        
            GameLog.Log($"Addressables:  FAILED Remove addressable folder {path} NOT FOUND");
        }
    }
}
