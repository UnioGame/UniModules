using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools
{
    using UniCore.EditorTools.Editor.Utility;
    using UniCore.Runtime.Utils;

    /// <summary>
    /// Odin Inspector extensions methods
    /// </summary>
    public static class OdinExtensions
    {
#if ODIN_INSPECTOR
        public static Func<object, Sirenix.OdinInspector.Editor.PropertyTree> PropertyTreeFactory = MemorizeTool.
            Create((object x) => Sirenix.OdinInspector.Editor.PropertyTree.Create(x),
                x => x.Dispose());
#endif
        
        public static Type UnityObjectType = typeof(Object);

#if ODIN_INSPECTOR
        public static Sirenix.OdinInspector.Editor.PropertyTree GetPropertyTree(this Object target)
        {
            return PropertyTreeFactory(target);
        }
#endif
     
        
        [Conditional("ODIN_INSPECTOR")]
        public static void DrawOdinPropertyInspector(this object asset)
        {
#if ODIN_INSPECTOR
            if (asset == null) {
                return;
            }
            
            var drawer = PropertyTreeFactory(asset);
            drawer.Draw(false);
#endif
        }

        public static Object DrawOdinPropertyField(
            this Object asset,
            Type type,
            Action<Object> onValueChanged,
            bool allowSceneObjects = true,
            string label = "")
        {
            var newValue = EditorGUILayout.ObjectField(label, asset, type, allowSceneObjects);
            if (newValue != asset) {
                onValueChanged?.Invoke(newValue);
            }

            try {
                newValue?.DrawOdinPropertyInspector();
            }
            catch (Exception r) {
            }

            return newValue;
        }

        public static bool DrawOdinPropertyWithFoldout(
            this Object asset,
            bool foldOut,
            string label = "",
            Action<Object> onValueChanged = null)
        {
            var targetType = asset == null ? null : asset.GetType();
            foldOut = EditorDrawerUtils.DrawObjectFoldout(asset, foldOut, targetType, label, x => {
                asset = x;
                onValueChanged?.Invoke(x);
            });

            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch (Exception r) {
            }

            return foldOut;
        }

        public static bool DrawOdinPropertyWithFoldout(
            this Object asset,
            Rect position,
            bool foldOut,
            string label = "")
        {
            var targetType = asset == null ? null : asset.GetType();
            if (targetType == null || asset == null) {
                EditorDrawerUtils.DrawDisabled(() => { EditorGUI.ObjectField(position, label, asset, UnityObjectType, true); });
                return false;
            }

            foldOut = EditorGUI.Foldout(position, foldOut, string.Empty);
            var rect = GUILayoutUtility.GetLastRect();
            rect.x += 14;

            EditorDrawerUtils.DrawDisabled(() => { EditorGUI.ObjectField(rect, label, asset, targetType, true); });

            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch (Exception r) {
            }

            return foldOut;
        }

        public static bool OdinFieldFoldout(
            this SerializedProperty property,
            bool foldout,
            GUIContent label,
            bool includeChildren)
        {
            foldout = EditorDrawerUtils.
                DrawFieldFoldout(
                    property,foldout, 
                    label, includeChildren);
            
            if (foldout) {
                switch (property.propertyType) {
                    case SerializedPropertyType.ObjectReference:
                        var value = property.objectReferenceValue;
                        value.DrawOdinPropertyInspector();
                        break;
                }
            }

            return foldout;
        }


        public static bool DrawOdinPropertyWithFoldout(
            this SerializedProperty property,
            Rect position,
            bool foldOut)
        {
            return DrawOdinPropertyWithFoldout(property.objectReferenceValue, position, foldOut, property.name);
        }
    }
}