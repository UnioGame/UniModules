using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UniModules.UniGame.CodeWriter.Editor.UnityTools;
using UnityEditor;
using UnityEngine;

public class GitCleaner : EditorWindow
{
    private const string _ignorePathFilterPath = "UniGame.Generated/GitCleaner/ignore-list.txt";
    private static List<string> _ignoreContent = new List<string>();
    private static List<string> _removedItems = new List<string>();

    private List<string> _ignoredCheckBoxes = new List<string>();

    [InitializeOnLoadMethod]
    [MenuItem("Tools/Git/Cleaner")]
    static void Startup()
    {
        CheckEmptyFolders();
    }


    [MenuItem("UniGame/Git Cleaner/Check Empty Folders")]
    public static void CheckEmptyFolders()
    {
        var rootPath = Application.dataPath;
        var list     = new List<string>();
        DirectoryUtil.ListEmptyDirs(list, rootPath);
        
        if (!Validate(list)) {
            return;
        }

        var pathes = list.Select(p => p.Substring(rootPath.Length - "Assets".Length)).ToList();
        EditorApplication.delayCall += () => ShowWindow(pathes);
    }

    public static bool Validate(List<string> paths)
    {
        if (paths.Any() == false)
            return false;

        _removedItems.Clear();
        var ignoreList = ReloadIgnoreList();
        foreach (var path in paths) {
            if(string.IsNullOrEmpty(path))
                continue;
            if (ignoreList.All(x => path.IndexOf(x) < 0)) {
                return true;
            }
            _removedItems.Add(path);
        }

        foreach (var removedItem in _removedItems) {
            paths.Remove(removedItem);
        }

        return false;
    }

    static List<string> ReloadIgnoreList()
    {
        _ignorePathFilterPath.ReadUnityFile(out var content,true);
        _ignoreContent = content.
            Split('\n').
            ToList();
        return _ignoreContent;
    }
    
    static void ShowWindow(List<string> paths)
    {
        var window = GetWindow<GitCleaner>();
        window.titleContent = new GUIContent("Git Cleaner");
        window.Initialize(paths);
        window.minSize = new Vector2(450, 250);
        window.Show();
    }

    private List<string> directories = new List<string>();
    private Vector2 scroll;

    public void Initialize(List<string> targetDirectories)
    {
        directories.Clear();
        directories.AddRange(targetDirectories.
            Where(x => !_ignoreContent.Contains(x)));
    }
    
    private void OnGUI()
    {
        var message =
            "Git non-sync folders found in project.\n" +
            "Want to delete them?";

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Box(message, EditorStyles.largeLabel);
        }

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            var oldColor = GUI.color;
            GUI.color = Color.yellow;
            if (GUILayout.Button("DELETE ALL", GUI.skin.button, GUILayout.Width(100), GUILayout.Height(25)))
            {
                try
                {

                    AssetDatabase.DisallowAutoRefresh();
                    foreach (var d in directories)
                    {
                        AssetDatabase.DeleteAsset(d);
                    }

                }
                finally
                {
                    Close();
                    AssetDatabase.AllowAutoRefresh();
                }

                AssetDatabase.Refresh();

                Startup();
            }
            GUI.color = oldColor;

            if (GUILayout.Button("IGNORE", GUI.skin.button, GUILayout.Width(70), GUILayout.Height(25)))
            {
                _ignoreContent.AddRange(_ignoredCheckBoxes);
                string.Join("\n", _ignoreContent).WriteUnityFile(_ignorePathFilterPath);
                Close();
            }
        }

        scroll = GUILayout.BeginScrollView(scroll, false, true);
        foreach (var directory in directories)
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button(directory, EditorStyles.helpBox, GUILayout.MinWidth(100)))
            {
                EditorUtil.PingFolder(directory);
            }

            var isToggleActive = _ignoredCheckBoxes.Contains(directory);
            var newState = GUILayout.Toggle(isToggleActive, string.Empty,GUILayout.ExpandWidth(false));
;           if(newState != isToggleActive)
            {
                if (newState) {
                    _ignoredCheckBoxes.Add(directory);
                }
                else {
                    _ignoredCheckBoxes.Remove(directory);
                }
            }

GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    static class EditorUtil
    {
        public static void PingFolder(string path)
        {
            if (path[path.Length - 1] == '/')
                path = path.Substring(0, path.Length - 1);

            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            EditorGUIUtility.PingObject(obj);
        }
    }

    static class DirectoryUtil
    {
        public static void ListEmptyDirs(List<string> list, string directory)
        {
            try
            {
                foreach (var d in Directory.EnumerateDirectories(directory))
                {
                    ListEmptyDirs(list, d);
                }

                var entries = Directory.EnumerateFileSystemEntries(directory);
                if (!entries.Any())
                {
                    list.Add(directory);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

    }
}