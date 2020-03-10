using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools
{
    using UniCore.EditorTools.Editor.Utility;

    /// <summary>
    /// Odin Inspector extensions methods
    /// </summary>
    public static class OdinExtensions
    {
        public static Type UnityObjectType = typeof(Object);
    
        [Conditional("ODIN_INSPECTOR")]
        public static void DrawOdinPropertyInspector(this object asset)
        {
            if (asset == null) return;

            using (var drawer = Sirenix.OdinInspector.Editor.PropertyTree.Create(asset)) {
                drawer.Draw(false);
            }

        }
        
        public static bool DrawOdinPropertyWithFoldout(
            this Object asset,
            bool foldOut,
            string label = "")
        {
            var targetType = asset == null ? null : asset.GetType();
            foldOut = EditorDrawerUtils.DrawObjectFoldout(asset, foldOut,targetType, label);
            
            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch(Exception r) { }

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
                EditorDrawerUtils.DrawDisabled(() => {
                    EditorGUI.ObjectField(position,label, asset, UnityObjectType, true);
                });
                return false;
            }

            foldOut = EditorGUI.Foldout(position,foldOut,string.Empty);            
            var rect = GUILayoutUtility.GetLastRect();
            rect.x += 14;
            
            EditorDrawerUtils.DrawDisabled(() => {
                EditorGUI.ObjectField(rect,label, asset, targetType, true);
            });
            
            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch(Exception r) { }

            return foldOut;
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
