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
        /// <summary>
        /// Gets all childrens of `SerializedObjects`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedObject serializedProperty)
        {
            var property = serializedProperty.GetIterator();
            return property.GetChildrens();
        }

        /// <summary>
        /// Gets all childrens of `SerializedObjects`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(
            this SerializedObject serializedProperty, 
            bool includeChilds = false)
        {
            var property = serializedProperty.GetIterator();
            return property.GetVisibleChildren(includeChilds);
        }

        /// <summary>
        /// Gets all children of `SerializedProperty`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildrens(
            this SerializedProperty serializedProperty)
        {
            var currentProperty     = serializedProperty.Copy();
            
            if (!currentProperty.Next(true)) {
                yield break;
            }
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.Next(false));
        }
        
        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildren(
            this SerializedProperty serializedProperty, 
            bool includeChilds)
        {
            foreach (var property in GetVisibleChildren(serializedProperty)) {
                yield return property;
                if(!includeChilds) continue;
                foreach (var visibleChild in property.GetChildren(includeChilds)) {
                    yield return visibleChild;
                }
            }
        }
        
        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(
            this SerializedProperty serializedProperty, 
            bool includeChilds)
        {
            foreach (var property in GetVisibleChildren(serializedProperty)) {
                yield return property;
                if(!includeChilds) continue;
                foreach (var visibleChild in property.GetVisibleChildren(includeChilds)) {
                    yield return visibleChild;
                }
            }
        }

        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
        {
            var currentProperty     = serializedProperty.Copy();
            
            if (!currentProperty.NextVisible(true)) {
                yield break;
            }
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.NextVisible(false));
        }
        
        
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