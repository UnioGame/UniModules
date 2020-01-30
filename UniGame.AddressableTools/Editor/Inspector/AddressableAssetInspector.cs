namespace UniGreenModules.UniGame.AddressableTools.Editor.Inspector
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Runtime.Attributes;
    using UniCore.EditorTools.Editor.PropertiesDrawers;
    using UniCore.EditorTools.Editor.Utility;
    using UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(ShowAssetReferenceAttribute),true)]
    public class AddressableAssetInspector : PropertyDrawer
    {
        private const float FieldHeight = 20;
        private const string assetLabel = "Asset";
        private const string guiPropertyName = "m_AssetGUID";

        private static Dictionary<FieldInfo,PropertyDrawer> drawers = new Dictionary<FieldInfo, PropertyDrawer>();
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => FieldHeight + base.GetPropertyHeight(property, label);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, new GUIContent(), property);
            
            DrawOnGuiAssetReferenceInspector(position,property);

            DrawAssetReferenceDrawer(position, property, label);

            EditorGUI.EndProperty();
        }

        private void DrawAssetReferenceDrawer(Rect position, SerializedProperty property, GUIContent label)
        {
            position.y      += FieldHeight;
            position.height =  20f;

            if (!drawers.TryGetValue(fieldInfo,out var drawer)) {
                var drawerType = typeof(AssetReference);
                drawer     = fieldInfo.GetDrawer(drawerType);
                drawers[fieldInfo] = drawer;
            }
            
            drawer.OnGUI(position,property,label);
        }

        public void DrawOnGuiAssetReferenceInspector(Rect position, SerializedProperty property)
        {
            position.height = 20;

            var guidProperty = property.FindPropertyRelative(guiPropertyName);
            var assetGuid    = guidProperty.stringValue;
            if (string.IsNullOrEmpty(assetGuid))
            {
                EditorGUI.ObjectField(position,assetLabel, null, typeof(Object),false);  
                return;
            }
            
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var mainType  = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            var asset     = AssetDatabase.LoadAssetAtPath(assetPath, mainType);

            EditorGUI.ObjectField(position,assetLabel, asset, asset.GetType(),false);

            
        }
    }
}
