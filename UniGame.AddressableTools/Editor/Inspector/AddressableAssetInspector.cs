using UnityEditor;

namespace UniGreenModules.AddressableTools.Editor
{
    using System;
    using System.Reflection;
    using UniCore.EditorTools.Editor.PropertiesDrawers;
    using UniGame.AddressableTools.Runtime.Attributes;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CustomPropertyDrawer(typeof(ShowAssetReferenceAttribute),true)]
    public class AddressableAssetInspector : PropertyDrawer
    {
        
        private const string guiPropertyName = "m_AssetGUID";
        private const string fieldInfoName = "m_FieldInfo";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var drawerType = typeof(AssetReference);
            var drawer     = property.GetDrawer(drawerType);
            var field =drawer.GetType().GetField(fieldInfoName, 
                BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(drawer,fieldInfo);

            drawer.OnGUI(position,property,label);
            
            DrawOnGuiAssetReferenceInspector(position,property,label);

        }

        public void DrawOnGuiAssetReferenceInspector(Rect position, SerializedProperty property, GUIContent label)
        {
            var guidProperty = property.FindPropertyRelative(guiPropertyName);
            var assetGuid    = guidProperty.stringValue;
            if (string.IsNullOrEmpty(assetGuid)) return; 
            
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var mainType  = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            var asset     = AssetDatabase.LoadAssetAtPath(assetPath, mainType);

            EditorGUILayout.ObjectField("Asset:",asset, asset.GetType(),false);

        }
    }
}
