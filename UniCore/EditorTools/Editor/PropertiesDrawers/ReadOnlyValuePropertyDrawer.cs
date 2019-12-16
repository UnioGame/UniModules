using UniGreenModules.UniCore.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace UniGreenModules.UniCore.EditorTools.Editor.PropertiesDrawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyValueAttribute))]
    public class ReadOnlyValuePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
            GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
 
        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}