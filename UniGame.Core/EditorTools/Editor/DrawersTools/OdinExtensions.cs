using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools
{
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
    
        public static bool DrawOdinPropertyWithFoldout(this SerializedProperty property, 
            Rect position, bool foldOut)
        {
            var targetType = Type.GetType(property.type,false,true);
            if (targetType == null || UnityObjectType.IsAssignableFrom(targetType) == false)
                return false;

            var asset = property.objectReferenceValue;
            if (asset == null) return false;

            foldOut = EditorGUI.Foldout(position,foldOut,string.Empty);
            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch(Exception r) { }

            return foldOut;
        }

    
    }
}
