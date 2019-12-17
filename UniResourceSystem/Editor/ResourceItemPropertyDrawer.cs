namespace UniGreenModules.UniResourceSystem.Editor
{
    using System.Linq;
    using Runtime;
    using UniCore.EditorTools.Editor;
    using UniCore.Runtime.Attributes;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

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
            
            var typeAttribute = property.GetAttributes<TargetTypeAttribute>().FirstOrDefault() as TargetTypeAttribute;

            var type = typeAttribute != null ? typeAttribute.TargetType : typeof(Object);
            
            assetProperty.objectReferenceValue = 
                EditorGUILayout.ObjectField("Target",assetProperty.objectReferenceValue,type,true);
            
            EditorGUILayout.Separator();
            
            EditorGUI.EndProperty();

            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
