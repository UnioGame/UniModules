using UnityEditor;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.AddressableTools.Editor
{
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    using Editor = UnityEditor.Editor;

    [CustomPropertyDrawer(typeof(AssetReference),true)]
    public class AddressableAssetInspector : PropertyDrawer
    {
        private const string guiPropertyName = "m_AssetGUID";
        
        private AssetReference assetReference;

  
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            
            var baseVisualElement = base.CreatePropertyGUI(property);
            container.Add(baseVisualElement);
            
            var imguiContainer = DrawAddressableAssetInspector(property);
            container.Add(imguiContainer);
            
            return container;
            
        }


        public VisualElement DrawAddressableAssetInspector(SerializedProperty property)
        {
            var container = new VisualElement();
            var assetField = new ObjectField("target:");
            assetField.allowSceneObjects = false;
            
            container.Add(assetField);
            
            var guidProperty = property.FindPropertyRelative(guiPropertyName);
            var assetGuid = guidProperty.stringValue;
            if (string.IsNullOrEmpty(assetGuid)) return container; 
            
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var mainType  = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, mainType);
            
            
            assetField.value = asset;

            return container;
        }
    }
}
