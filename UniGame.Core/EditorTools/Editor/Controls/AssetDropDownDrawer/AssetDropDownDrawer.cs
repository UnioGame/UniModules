namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Attributes;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(AssetDropDownAttribute))]
    public class AssetDropDownDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";
        
        private static List<string> assetsItems = new List<string>();
        private static Func<AssetDropDownAttribute, List<Object>> typeAssets = MemorizeTool.Create<AssetDropDownAttribute,List<Object>>(FindFiltered);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetAttribute = attribute as AssetDropDownAttribute;

            var filterType = targetAttribute.FilterType == null ?
                fieldInfo.FieldType : targetAttribute.FilterType;
            
            var folderFilter = targetAttribute.FolderFilter;
            
            assetsItems.Clear();
            assetsItems.Add(emptyValue);
            
            var searchTarget = new AssetDropDownAttribute() {
                FilterType = filterType,
                FolderFilter = folderFilter,
                FoldOutOpen = targetAttribute.FoldOutOpen
            };

            var assets = typeAssets(searchTarget);
            
            var target = property.objectReferenceValue;
            var currentValue = assets.FirstOrDefault(x => target == x);
            var index = assets.IndexOf(currentValue) + 1;

            assetsItems.AddRange(assets.Select(x => x.name));
            
            var controlPosition = position;
            EditorGUI.PropertyField(controlPosition, property, label,true);
            
            var fieldRect = EditorGUILayout.GetControlRect();
            controlPosition   =  fieldRect;
            var newIndex = EditorGUI.Popup(controlPosition, 
                string.Empty, 
                index, 
                assetsItems.ToArray());
            
            position.height += fieldRect.height;
            
            if (newIndex == index) {
                return;
            }

            var targetIndex = newIndex - 1;
            property.objectReferenceValue = targetIndex < 0 ? null : assets[targetIndex];
            property.serializedObject.ApplyModifiedProperties();

        }

        private static List<Object> FindFiltered(AssetDropDownAttribute targetAttribute)
        {
            var searchAsset = new List<Object>();
            return AssetEditorTools.FindAssets(searchAsset, targetAttribute.FilterType,
                string.IsNullOrEmpty(targetAttribute.FolderFilter) ? 
                    null :
                    new []{targetAttribute.FolderFilter});
        }
        
    }
}
