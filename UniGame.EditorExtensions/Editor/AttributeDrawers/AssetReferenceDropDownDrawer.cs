namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
    using UniModules.UniGame.EditorExtensions.Runtime.Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(AssetReferenceDropDownAttribute))]
    public class AssetReferenceDropDownDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";

        private static Func<Type, List<Type>> assetReferenceMap = MemorizeTool.Create<Type, List<Type>>(GetReferenceClassTypes);
        private static Func<Type, List<string>> typeNamesReferenceMap = MemorizeTool.Create<Type, List<String>>(GetTypeItemsNames);

        private static List<string> assetsItems = new List<string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target          = property.objectReferenceValue;
            var targetAttribute = attribute as IAssetReferenceDropDownInfo;
            
            //if attribute filter type empty use actual field type
            var filterType = targetAttribute.BaseType == null ?
                fieldInfo.FieldType : 
                targetAttribute.BaseType;

            var targetReferenceTypes = assetReferenceMap(filterType);
            assetsItems.Clear();
            assetsItems.Add(emptyValue);
            assetsItems.AddRange(typeNamesReferenceMap(filterType));

            var controlPosition = position;
            EditorGUI.PropertyField(controlPosition, property, label,true);
            
            // var fieldRect = EditorGUILayout.GetControlRect();
            // controlPosition   =  fieldRect;
            // var newIndex = EditorGUI.Popup(controlPosition, 
            //     string.Empty, 
            //     index, 
            //     assetsItems.ToArray());
            //
            // position.height += fieldRect.height;
            //
            // if (newIndex == index) {
            //     return;
            // }
            //
            // var targetIndex = newIndex - 1;
            // property.objectReferenceValue = targetIndex < 0 ? null : targetReferenceTypes[targetIndex];
            // property.serializedObject.ApplyModifiedProperties();

        }

        public static List<Type> GetReferenceClassTypes(Type baseType)
        {
            var assignableTypes = baseType.GetAssignableTypes();

            assignableTypes = assignableTypes.
                Where(x => x.IsInterface == false).
                Where(x => x.HasDefaultConstructor()).
                ToList();
            
            return assignableTypes;
        }

        public static List<string> GetTypeItemsNames(Type baseType) => assetReferenceMap(baseType).
            Select(x => x.Name).
            ToList();

    }
}
