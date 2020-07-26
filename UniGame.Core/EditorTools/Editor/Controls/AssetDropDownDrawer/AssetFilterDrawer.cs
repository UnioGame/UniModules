namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DrawersTools;
    using Runtime.Attributes;
    using Runtime.Extension;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(AssetFilterAttribute))]
    public class AssetFilterDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";
        
        private static List<string> assetsItems = new List<string>();
        private static Func<AssetFilterAttribute, List<Object>> typeAssets = MemorizeTool.Create<AssetFilterAttribute,List<Object>>(FindFiltered);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetAttribute = attribute as AssetFilterAttribute;

            var filterType = targetAttribute.FilterType == null ?
                fieldInfo.FieldType : targetAttribute.FilterType;
            
            var folderFilter = targetAttribute.FolderFilter;
            
            assetsItems.Clear();
            assetsItems.Add(emptyValue);
            
            var searchTarget = new AssetFilterAttribute() {
                FilterType = filterType,
                FolderFilter = folderFilter,
                FoldOutOpen = targetAttribute.FoldOutOpen,
                DrawWithOdin = targetAttribute.DrawWithOdin
            };

            var assets = typeAssets(searchTarget);
            
            var target = property.objectReferenceValue;
            var currentValue = assets.FirstOrDefault(x => target == x);
            var index = assets.IndexOf(currentValue) + 1;

            assetsItems.AddRange(assets.Select(x => x.name));
            
            var controlPosition = position;
            
            EditorGUI.PropertyField(controlPosition, property, label,true);

#if ODIN_INSPECTOR
            if (searchTarget.DrawWithOdin) {
                var assetValue = property.objectReferenceValue;
                if (assetValue) {
                    assetValue.DrawOdinPropertyInspector();
                }
            }
#endif

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

        private static List<Object> FindFiltered(AssetFilterAttribute targetAttribute)
        {
            var isObject = targetAttribute.FilterType.IsAsset();
            var filterType = targetAttribute.FilterType;
            var folderFilter = string.IsNullOrEmpty(targetAttribute.FolderFilter) ? null : new[] {targetAttribute.FolderFilter};
            if (isObject) {
                return AssetEditorTools.GetAssets(filterType, folderFilter);
            }
            return AssetEditorTools.
                GetAssets(UnityTypeExtension.scriptableType,folderFilter).
                Where(x => filterType.IsInstanceOfType(x)).
                ToList();
        }
        
    }
}
