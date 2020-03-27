namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    [UiElementsDrawer(-1)]
    public class OdinAssetUiElementsDrawer : UiElementsTypeDrawer
    {
        public override bool IsTypeSupported(Type type)
        {
            return UiElementFactory.ScriptableType.IsAssignableFrom(type) ||
                UiElementFactory.ComponentType.IsAssignableFrom(type);
        }

        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {
            var control = new OdinValueView() {
                Label       = label,
                Value       = source as Object,
                IsOpen      = false,
                AssetAction = onValueChanged,
            };
            return control.View;
        }
    }
}