using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelEditor
{
    public class EditorDrawerUtils {


        public static void DrawVertialLayout(Action action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            try
            {
                if (options == null)
                {
                    EditorGUILayout.BeginVertical(options);
                }
                else
                {
                    EditorGUILayout.BeginVertical();
                }
                action();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
            
        }

        public static void DrawVertialLayout(Action action,Color color, params GUILayoutOption[] options)
        {
            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            DrawVertialLayout(action,options);
            GUI.backgroundColor = prevColor;
        }

        public static void DrawVertialLayout(Action action)
        {
            if (action == null) return;
            try
            {
                EditorGUILayout.BeginVertical();
                action();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
        }

        public static void DrawHorizontalLayout(Action action,params GUILayoutOption[] guiLayoutOptions)
        {
            if (action == null) return;
            try
            {
                if (guiLayoutOptions == null)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal(guiLayoutOptions);
                }
                action();
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
            
        }

        public static void DrawAndRevertColor(Action action)
        {
            var defaultColor = GUI.backgroundColor;
            action();
            GUI.backgroundColor = defaultColor;
        }

        public static Vector2 DrawScroll(Action drawer,Vector2 scroll)
        {
            if (drawer == null)
                return scroll;
            try
            {
                scroll = EditorGUILayout.BeginScrollView(scroll);
                drawer();
                return scroll;
            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }

        }

        public static Object DrawObject(Object item,bool opened = false)
        {
            var editor = item.GetEditor();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            var result = EditorGUILayout.ObjectField(item, item.GetType());
            editor.OnInspectorGUI();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            return result;
        }
    }
}
