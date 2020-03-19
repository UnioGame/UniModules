namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Attributes;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CustomPropertyDrawer(typeof(AssetDropDownAttribute))]
    public class AssetDropDownDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";
        
        private static List<Object> assets = new List<Object>();
        private static List<string> assetsItems = new List<string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target          = property.objectReferenceValue;
            var targetAttribute = attribute as AssetDropDownAttribute;
            
            var filterType = targetAttribute.FilterType == null ?
                fieldInfo.FieldType : targetAttribute.FilterType;
            
            var folderFilter = targetAttribute.FolderFilter;
            
            assets.Clear();
            assetsItems.Clear();
            assetsItems.Add(emptyValue);
            
            AssetEditorTools.FindAssets(assets, filterType,
                string.IsNullOrEmpty(folderFilter) ? 
                    null :
                    new []{folderFilter});

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
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var target = property.objectReferenceValue;
            var targetAttribute = attribute as AssetDropDownAttribute;
            
            var filterType = targetAttribute.FilterType == null ?
                fieldInfo.FieldType : targetAttribute.FilterType;
            var folderFilter = targetAttribute.FolderFilter;
            
            assets.Clear();

            AssetEditorTools.FindAssets(assets, filterType, 
                string.IsNullOrEmpty(folderFilter) ? 
                    null :
                    new []{folderFilter});

            var currentValue = assets.FirstOrDefault(x => target == x);
            
            // Each editor window contains a root VisualElement object
            var container = new VisualElement();
            var popup = new PopupField<Object>(assets, currentValue);
            container.style.marginTop = 60;
            { 
                container.Add(popup);
            }

            popup.RegisterValueChangedCallback(x => {
                property.objectReferenceValue = x.newValue;
            });
            
            return container;
        }
        
    }
}
