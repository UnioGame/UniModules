using UnityEditor;

namespace UniGreenModules.AddressableTools.Editor
{
    using UnityEngine;

    public class AddressableAssetInspector<TTarget> : PropertyDrawer
    {
        private const string guiPropertyName = "m_AssetGUID";
 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField( position, property, label, true );
            DrawOnGuiAssetReferenceInspector(property);
        }

        public void DrawOnGuiAssetReferenceInspector(SerializedProperty property)
        {
            var guidProperty = property.FindPropertyRelative(guiPropertyName);
            var assetGuid    = guidProperty.stringValue;
            if (string.IsNullOrEmpty(assetGuid)) return; 
            
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var mainType  = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            var asset     = AssetDatabase.LoadAssetAtPath(assetPath, mainType);

            EditorGUILayout.ObjectField("target:",asset, typeof(TTarget),false);

        }
    }
}
