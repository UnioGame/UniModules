using UnityEditor;

namespace Taktika.Editor.Editor.SerializableTypeEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameRuntime.Types;
    using GameRuntime.Types.Attributes;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class TypDrawer
    {

        private static Func<Type, List<Type>> getAssagnableTypes = MemorizeTool.Create<Type, List<Type>>(ReflectionTools.GetAssignableTypes);

        private const string noneValue = "none";
        
        private static List<string> popupValues = new List<string>();
        
        public static Type DrawTypePopup(Rect position,GUIContent label, Type baseType, Type selectedType)
        {
            //all assignable types
            var types = getAssagnableTypes(baseType);

            var selectedIndex = 0;
            
            popupValues.Clear();
            popupValues.Add(noneValue);
            for (var i = 0; i < types.Count; i++) {
                var item = types[i];
                var itemIndex = i + 1;
                
                popupValues.Add(item.Name);
                selectedIndex = item == selectedType ? 
                    itemIndex : selectedIndex;
            }
            
            var newSelection = EditorGUI.Popup(position, label.text, selectedIndex, popupValues.ToArray());

            return newSelection == 0 ? null : types[newSelection - 1];
        }
        
        
        
    }
    
    [CustomPropertyDrawer(typeof(STypeFilterAttribute))]
    public class STypeInspector : PropertyDrawer
    {
        private Type selection = null;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetAttribute = attribute as STypeFilterAttribute;
            var targetType      = targetAttribute?.Type;
            
            if (targetType == null) {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var fieldName      = targetAttribute.FieldName;
            //get type name field
            var targetProperty = property.FindPropertyRelative(fieldName);
            if (targetProperty == null) {
                Debug.LogError($"property field with name {fieldName} not found");
                return;
            }

            selection = Type.GetType(targetProperty.stringValue, false, true);
                
            var newSelection = TypDrawer.DrawTypePopup(position, label, targetType, selection);
            if (newSelection == selection) {
                return;
            }

            selection = newSelection;
            targetProperty.stringValue = selection == null ? string.Empty :selection.AssemblyQualifiedName;
            
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}