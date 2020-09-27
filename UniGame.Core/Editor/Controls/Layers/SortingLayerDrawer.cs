namespace UniModules.UniGame.Core.EditorTools.Editor.Controls.Layers
{
    using System.Reflection;
    using Runtime.Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
                return;
            
            var style = new GUIStyle(EditorStyles.popup);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, style);
            var content = new GUIContent(EditorGUIUtility.TrTextContent("Sorting Layer", "Name of the Renderer's sorting layer."));
            
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            SortingLayerField(rect, content, property, style, EditorStyles.label);
            EditorGUI.EndProperty();
        }

        private void SortingLayerField(Rect position, GUIContent label, SerializedProperty layerId, GUIStyle style, GUIStyle labelStyle)
        {
            var methodInfo = typeof(EditorGUI).GetMethod("SortingLayerField", BindingFlags.Static | BindingFlags.NonPublic,
                null, new[] {typeof(Rect), typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle), typeof(GUIStyle)}, null);
            if (methodInfo != null) {
                var parameters = new object[] {position, label, layerId, style, labelStyle};
                methodInfo.Invoke(null, parameters);
            }
        }
    }
}