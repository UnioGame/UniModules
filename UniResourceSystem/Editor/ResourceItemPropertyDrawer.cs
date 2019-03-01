using UniModule.UnityTools.ResourceSystem;
using UnityEditor;
using UnityEngine;

namespace Modules.UniTools.UniResourceSystem.Editor
{
    [CustomPropertyDrawer(typeof(ResourceItem))]
    public class ResourceItemPropertyDrawer :  PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var changed = false;
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            var assetProperty = property.FindPropertyRelative("asset");

            changed = EditorGUILayout.PropertyField(assetProperty) || changed;
            
            EditorGUILayout.Separator();
            
            EditorGUI.EndProperty();

            if (changed)
            {
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

        }
    }
}
