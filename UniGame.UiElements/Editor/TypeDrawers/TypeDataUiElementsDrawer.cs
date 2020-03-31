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
            
            var container    = new VisualElement();

            foreach (var pair in values.EditorValues) {
                var valueContainer = pair.Value;
                var objectReadOnlyValue    = valueContainer as IReadonlyObjectValue;
                var valueWriter = valueContainer as IObjectValue;
                var type           = pair.Key;

                if (objectReadOnlyValue == null)
                    continue;

                var value     = objectReadOnlyValue.GetValue();
                var valueType = objectReadOnlyValue.Type;
                var labelText = $"[Registered as :{type.Name}] : [Value Type :{valueType?.Name}]";

                var element = UiElementFactory.Create(
                    value,
                    valueType,
                    x => {
                        valueWriter?.SetObjectValue(x);
                        onValueChanged?.Invoke(source);
                    },
                    valueType?.Name);
                  
                var foldout = UiElementFactory.CreateFoldout(labelText,16);
                element = element == null ? new Label($"EMPTY") : element;

                foldout.Add(element);
                container.Add(foldout);

            }
            
            return container;
        }
    }
}