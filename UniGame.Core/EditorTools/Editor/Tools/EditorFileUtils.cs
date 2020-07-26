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
        private static string assetsFolderName = "Assets";
        
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
            a = a.Replace("\\", "/").TrimEnd('/');
            b = b.Replace("\\", "/").TrimStart('/');
            return a + "/" + b;
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
        
        public static string ValidateUnityPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return path.Replace("\\", "/").TrimEnd('/');
        }

        public static string[] SplitPath(this string path)
        {
            return path.Split('/').ToArray();
        }

        public static void ValidateDirectories(string sourcePath)
        {
            var directoryPath = Path.GetDirectoryName(sourcePath);
            var directories = directoryPath.ValidateUnityPath();
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
