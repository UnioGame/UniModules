namespace UniModules.UniGame.UiToolkit.Runtime.Extensions
{
    using UnityEngine.UIElements;

    public static class StyleExtensions 
    {
        public static VisualElement SwapClasses(this VisualElement element,string fromClass,string toClass)
        {
            element.RemoveFromClassList(fromClass);
            element.AddToClassList(toClass);
            return element;
        }

        public static VisualElement AddStyleSheet(this VisualElement element, StyleSheet styleSheet)
        {
            if (!styleSheet)
                return element;
            if (!element.styleSheets.Contains(styleSheet))
            {
                element.styleSheets.Add(styleSheet);
            }
            return element;
        }
        
    }
}
