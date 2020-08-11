namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Extension;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
    using UniModules.UniGame.Core.EditorTools.Editor.AssetOperations;
    using UniModules.UniGame.EditorExtensions.Runtime.Attributes;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(ImplementationDropDownAttribute))]
    public class AssetReferenceDropDownDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";

        private static Func<Type, List<Type>> assetReferenceMap = MemorizeTool.Create<Type, List<Type>>(GetReferenceClassTypes);
        private static Func<Type, List<string>> typeNamesReferenceMap = MemorizeTool.Create<Type, List<String>>(GetTypeItemsNames);
        private static Func<Type, List<Object>> typeNamesToAssetsMap = MemorizeTool.Create<Type, List<Object>>(GetAssetsByType);

        private static List<string> optionsNames = new List<string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetAttribute = attribute as IAssetDropDownInfo;
            
            //if attribute filter type empty use actual field type
            var filterType = targetAttribute.BaseType == null ?
                fieldInfo.FieldType : 
                targetAttribute.BaseType;

            //is managed type or unity asset
            var isAsset = filterType.IsAsset();

            if (isAsset) {
                DrawAssetsDropDown(position,property,filterType,label);
            }
            else {
                DrawReferenceDropDown(position,property,filterType,label);
            }
            
            
            property.serializedObject.ApplyModifiedProperties();

        }


        public void DrawAssetsDropDown(Rect position,SerializedProperty property,Type filterType, GUIContent label)
        {
                        
            optionsNames.Clear();
            optionsNames.Add(emptyValue);
            optionsNames.AddRange(typeNamesReferenceMap(filterType));

            //select active options index
            var index = 0;
            var asset = property.objectReferenceValue;
            var assetItems = typeNamesToAssetsMap(filterType);
            
            if (asset != null) {
                index = assetItems.IndexOf(asset) + 1;
            }

            var controlPosition = position;
            
            var newIndex = EditorGUI.Popup(controlPosition, 
                string.Empty, 
                index, 
                optionsNames.ToArray());
            
            var fieldRect = EditorGUILayout.GetControlRect();
            controlPosition = fieldRect;
            
            if (newIndex != index) {
                var targetIndex = newIndex - 1;
                property.objectReferenceValue = targetIndex < 0 && assetItems.Count > 0 ? null : assetItems[targetIndex];
            }
            
            EditorGUI.PropertyField(controlPosition, property, label,true);
            
            position.height += fieldRect.height;
            
        }

        public void DrawReferenceDropDown(Rect position,SerializedProperty property,Type filterType, GUIContent label)
        {
                        
            optionsNames.Clear();
            optionsNames.Add(emptyValue);
            optionsNames.AddRange(typeNamesReferenceMap(filterType));

            //select active options index
            var referenceTypes = assetReferenceMap(filterType);
            var index = optionsNames.IndexOf(property.managedReferenceFieldTypename);

            var controlPosition = position;
            
            var newIndex = EditorGUI.Popup(controlPosition, 
                string.Empty, 
                index, 
                optionsNames.ToArray());

            if (index != newIndex) {
                property.managedReferenceValue = newIndex <= 0 ? null : 
                    Activator.CreateInstance(referenceTypes[newIndex-1]);
            }
            
            var fieldRect = EditorGUILayout.GetControlRect();
            controlPosition = fieldRect;
            
            EditorGUI.PropertyField(controlPosition, property, label,true);
            
            position.height += fieldRect.height;

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

        public static List<string> GetTypeItemsNames(Type baseType)
        {
            List<string> result = null;
            
            //is managed type or unity asset
            if (baseType.IsAsset()) {
                result = typeNamesToAssetsMap(baseType).
                    Select(x => x.name).
                    ToList();
            }
            else {
                result = assetReferenceMap(baseType).
                    Select(x => x.Name).
                    ToList();
            }

            
            return result;
        }

        public static List<Object> GetAssetsByType(Type baseType)
        {
            
            var result = AssetEditorTools.GetAssets<Object>(baseType);
            return result;

        }
        
    }
}
