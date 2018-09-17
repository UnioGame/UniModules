using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelEditor
{
    public class EditorDrawerUtils : MonoBehaviour {


        public static void DrawVertialLayout(Action action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            EditorGUILayout.BeginVertical(options);
            action();
            EditorGUILayout.EndVertical();
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
            EditorGUILayout.BeginVertical();
            action();
            EditorGUILayout.EndVertical();
        }

        public static void DrawHorizontalLayout(Action action,params GUILayoutOption[] guiLayoutOptions)
        {
            if (action == null) return;
            if (guiLayoutOptions == null)
            {
                EditorGUILayout.BeginHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal(guiLayoutOptions);
            }
            action();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawAndRevertColor(Action action)
        {
            var defaultColor = GUI.backgroundColor;
            action();
            GUI.backgroundColor = defaultColor;
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
