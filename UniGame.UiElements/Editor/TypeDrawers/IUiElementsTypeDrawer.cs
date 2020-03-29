namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using UnityEngine.UIElements;

    public interface IUiElementsTypeDrawer
    {
        bool IsTypeSupported(Type type);
        VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null);
    }
}