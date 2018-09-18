using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelEditor
{
    public static class EditorExtension
    {

        public static void DrawDefaultEditor(this GameObject target)
        {
            if (!target) return;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
        }

        public static void DrawDefaultEditor(this Object target)
        {
            if (!target) return;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
        }

        public static IEnumerable<Type> GetClassItems(Type type)
        {
            return Assembly.GetAssembly(type).GetTypes().
                Where(t => t.IsSubclassOf(type));
        }

        public static Editor ShowCustomEditor(this Object target)
        {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            editor.OnInspectorGUI();
            return editor;
        }

        public static Editor GetEditor(this Object target)
        {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            return editor;
        }


    }
}
