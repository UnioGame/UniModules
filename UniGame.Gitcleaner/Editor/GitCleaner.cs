using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class GitCleaner : EditorWindow
{
    [InitializeOnLoadMethod]
    [MenuItem("Tools/Git/Cleaner")]
    static void Startup()
    {
        ThreadPool.QueueUserWorkItem(CheckEmptyFolders, Application.dataPath);
    }

    static void CheckEmptyFolders(object pathObj)
    {
        var rootPath = (string)pathObj;
        var list = new List<string>();
        DirectoryUtil.ListEmptyDirs(list, rootPath);
        if (list.Any())
        {
            var pathes = list.Select(p => p.Substring(rootPath.Length - "Assets".Length)).ToList();
            EditorApplication.delayCall += () => ShowWindow(pathes);
        }
    }

    static void ShowWindow(List<string> pathes)
    {
        var window = GetWindow<GitCleaner>();
        window.titleContent = new GUIContent("Git Cleaner");
        window.directories = pathes;
        window.minSize = new Vector2(450, 250);
        window.Show();
    }

    private List<string> directories;
    private Vector2 scroll;

    private void OnGUI()
    {
        var message =
            "В проекте обнаружены пустые папки не синхронизируемые Git.\n" +
            "Хотите удалить их?";

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
                foreach (var d in directories)
                {
                    AssetDatabase.DeleteAsset(d);
                }

                Close();
                AssetDatabase.Refresh();

                Startup();
            }
            GUI.color = oldColor;

            if (GUILayout.Button("IGNORE", GUI.skin.button, GUILayout.Width(70), GUILayout.Height(25)))
            {
                Close();
            }
        }

        scroll = GUILayout.BeginScrollView(scroll, false, true);
        foreach (var directory in directories)
        {
            if (GUILayout.Button(directory, EditorStyles.helpBox, GUILayout.MinWidth(100)))
            {
                EditorUtil.PingFolder(directory);
            }
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