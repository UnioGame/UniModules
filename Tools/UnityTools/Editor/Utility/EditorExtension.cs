using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.UI.Windows.Tools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelEditor {
    public static class EditorExtension {
        public static void DrawDefaultEditor(this GameObject target) {
            if (!target) return;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
        }

        public static Editor ShowDefaultEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
            return editor;
        }

        public static IEnumerable<Type> GetClassItems(Type type) {
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsSubclassOf(type));
        }

        public static Editor ShowCustomEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            editor.OnInspectorGUI();
            return editor;
        }

        public static TTarget AddNeted<TTarget>(this ScriptableObject root, string name = null)
            where TTarget : ScriptableObject {
            return AssetEditorTools.SaveAssetAsNested<TTarget>(root, name);
        }

        public static Editor GetEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            return editor;
        }

        public static void Save(this SerializedObject target) {
            target.ApplyModifiedProperties();
            EditorUtility.SetDirty(target.targetObject);
        }
    }
}