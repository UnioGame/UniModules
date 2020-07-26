namespace UniModules.UniGame.Core.EditorTools.Editor.Controls.Layers
{
    using Runtime.Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
                return;

            var style = new GUIStyle(EditorStyles.popup);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, style);
            var content = new GUIContent(EditorGUIUtility.TrTextContent("Layer",
                "The layer that this GameObject is in.\n\nChoose Add Layer... to edit the list of available layers."));
            
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            property.intValue = EditorGUI.LayerField(rect, content, property.intValue, style);
            EditorGUI.EndProperty();
        }
    }
}