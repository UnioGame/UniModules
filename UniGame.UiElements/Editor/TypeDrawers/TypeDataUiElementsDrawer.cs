namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces.Rx;
    using UnityEngine.UIElements;

    [UiElementsDrawer(1000)]
    public class TypeDataUiElementsDrawer : UiElementsTypeDrawer{
        public override bool IsTypeSupported(Type type)
        {
            return typeof(ITypeData).IsAssignableFrom(type);
        }

        public override VisualElement Draw(object source, Type itemType, string label = "", Action<object> onValueChanged = null)
        {
            var values = source as ITypeData;
            
            var container    = new ListView();
            var foldOutItems = new ListView();

            foreach (var pair in values.EditorValues) {
                var valueContainer = pair.Value;
                var objectValue    = valueContainer as IObjectValue;
                var type           = pair.Key;

                if (valueContainer.HasValue == false || objectValue == null)
                    continue;

                var value     = objectValue.GetValue();
                var valueType = objectValue.Type;

                var foldout = UiElementFactory.
                    CreateFoldout($"[Registered Type :{type.Name}] : [Value Type :{valueType?.Name}]");

                foldout.Add(foldOutItems);
                container.Add(foldout);

                var element = UiElementFactory.CreateVisualElement(
                    value,
                    valueType,
                    x => { },
                    valueType?.Name);
                
                //is empty value or value already shown
                if (element == null)
                    continue;
  
                foldOutItems.Add(element);
            }
            
            return container;
        }
    }
}