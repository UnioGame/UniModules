using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelEditor {
    
    public static class EditorDrawerUtils {
        
        public static void DrawVertialLayout(Action action, params GUILayoutOption[] options) {
            if (action == null) return;
            if (options == null) {
                EditorGUILayout.BeginVertical(options);
            }
            else {
                EditorGUILayout.BeginVertical();
            }

            action();

            EditorGUILayout.EndVertical();
        }

        public static void DrawVertialLayout(Action action, Color color, params GUILayoutOption[] options) {
            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            DrawVertialLayout(action, options);
            GUI.backgroundColor = prevColor;
        }

        public static void DrawVertialLayout(Action action) {
            if (action == null) return;

            EditorGUILayout.BeginVertical();
            action();
        }

        public static void DrawHorizontalLayout(Action action, params GUILayoutOption[] guiLayoutOptions) {
            
            if (action == null) return;
            if (guiLayoutOptions == null) {
                EditorGUILayout.BeginHorizontal();
            }
            else {
                EditorGUILayout.BeginHorizontal(guiLayoutOptions);
            }

            action();
            
            EditorGUILayout.EndHorizontal();
            
        }

        public static void DrawAndRevertColor(Action action) {
            var defaultBackColor = GUI.backgroundColor;
            var defaultGuiColor = GUI.color;
            var defaultContentColor = GUI.contentColor;
            action();
            GUI.backgroundColor = defaultBackColor;
            GUI.color = defaultGuiColor;
            GUI.contentColor = defaultContentColor;
        }
        
        public static void DrawWithBackgroudColor(Color color,Action action) {

            if (action == null) return;
            
            DrawAndRevertColor(() => {
                
                GUI.contentColor = color;
                action();

            });
            
        }

        public static Vector2 DrawScroll(Vector2 scroll,Action drawer) {
            
            if (drawer == null)
                return scroll;
            
            scroll = EditorGUILayout.BeginScrollView(scroll);
            
            drawer();

            EditorGUILayout.EndScrollView();

            return scroll;
        }

        public static Object DrawObject(Object item, bool opened = false) {
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