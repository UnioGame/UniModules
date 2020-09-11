using System.IO;
using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.EditorTools.Editor.Tools
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public static class EditorFileUtils
    {
        public const string FileRefExpr = @"[\w|\W]*[\/|\\](?<filename>[\w]+)+\.(\w+)";

        private static string assetsFolderName       = "Assets";
        private static char moveDirectorySeparator = '/';
        public static Regex FileRegExpr = new Regex(FileRefExpr,RegexOptions.Compiled);
        
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

            CreateDirectories(path);

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
        public static bool ReplaceDirectory(string source, string dest)
        {
            if(string.IsNullOrEmpty(source) || Directory.Exists(source) == false)
                return false;
            if (string.IsNullOrEmpty(dest))
                return false;

            if (source.Equals(dest, StringComparison.OrdinalIgnoreCase))
                return true;

            dest   = dest.FixDirectoryPath();

            try {
                
                //create all sub directories
                CreateDirectories(dest);
                //remote target
                DeleteDirectory(dest);

                return MoveDirectory(source,dest);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return false;
        }

        public static bool MoveDirectory(string source, string dest)
        {
            if (Directory.Exists(source) == false || Directory.Exists(dest))
                return false;
            
            try {
                Directory.Move(source,dest);
                
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

        public static bool IsFilePath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            var last = path[path.Length - 1];
            if (last == Path.PathSeparator || last == '\\' || last == '/')
                return false;
            if (FileRegExpr.IsMatch(path))
                return true;
            return File.Exists(path);
        }

        public static void CreateDirectories(string sourcePath, bool isFilePath = false)
        {
            if (string.IsNullOrEmpty(sourcePath))
                return;
            
            var isFile    = isFilePath || IsFilePath(sourcePath);
            var directory = new DirectoryInfo(sourcePath);
            var directoryPath = isFile ? 
                Path.GetDirectoryName(sourcePath) :
                directory.FullName;
            
            var directories   = directoryPath.FixUnityPath();
            var path          = string.Empty;
            var applyRefresh  = false;
            
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
                    applyRefresh = true;
                    Directory.CreateDirectory(path);
                }
            }
            
            if(applyRefresh) AssetDatabase.Refresh();
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
