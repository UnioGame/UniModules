namespace UniGreenModules.UniCore.EditorTools.Editor.Utility {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AssetOperations;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class EditorExtension {
        
        public static void AddToEditorSelection(this Object item, bool add)
        {
            if (item == null) return;
            
            if (add)
            {
                var selection = new List<Object>(Selection.objects);
                selection.Add(item);
                Selection.objects = selection.ToArray();
            }
            else Selection.objects = new Object[] {item};
        }

        public static void SetDirty(this object asset)
        {
            if(asset is Object assetObject)
                EditorUtility.SetDirty(assetObject);
        }

        public static bool IsSelected(this Object item) => item != null && Selection.Contains(item);
        
        public static bool IsSelected(this object item) => (item as Object).IsSelected();

        public static void PingInEditor(this Object item,bool markAsActive = true)
        {
            if (!item) return;
            
            EditorGUIUtility.PingObject( item );
            if (markAsActive) {
                Selection.activeObject = item;
            }
            
        }
        
        public static void DeselectFromEditor(this Object item)
        {
            if (item == null) return;
            var selection = new List<Object>(Selection.objects);
            selection.Remove(item);
            Selection.objects = selection.ToArray();
        }
        
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

        public static void DestroyNestedAsset(this Object asset) {
            
            Object.DestroyImmediate(asset,true);
            AssetDatabase.SaveAssets();
            
        }
        
        public static TTarget AddNeted<TTarget>(this ScriptableObject root, string name = null)
            where TTarget : ScriptableObject {
            return AssetEditorTools.SaveAssetAsNested<TTarget>(root, name);
        }
        
        public static Object AddNeted(this ScriptableObject root,Type assetType, string name = null)
        {
            return AssetEditorTools.SaveAssetAsNested(root,assetType, name);
            
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