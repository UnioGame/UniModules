namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using System.Reflection;
    using UnityEngine.UIElements;

    public interface ITypeValidator
    {
        bool IsTypeSupported(Type type);
    }
    
    public interface IUiElementsFieldDrawer : ITypeValidator
    {
        VisualElement Draw(
            object source,FieldInfo fieldInfo,
            Type type, string label = "", Action<object> onValueChanged = null);
    }

    public interface IUiElementsTypeDrawer : ITypeValidator
    {
        VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null);
    }
}