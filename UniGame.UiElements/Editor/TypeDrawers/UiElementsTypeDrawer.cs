namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using UnityEngine.UIElements;

    public abstract class UiElementsTypeDrawer : IUiElementsTypeDrawer
    {
        public abstract bool IsTypeSupported(Type type);

        public abstract VisualElement Draw(
            object source,
            Type type,
            string label = "",
            Action<object> onValueChanged = null);
    }
}