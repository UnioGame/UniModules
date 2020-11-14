namespace UniModules.UniGame.UiToolkit.Runtime.Styles
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "UniGame/Ui Toolkit/StylesMapAsset",fileName = nameof(StyleMapAsset))]
    public class StyleMapAsset : ScriptableObject
    {
        
        public static readonly StyleSheet DefaultStyle = ScriptableObject.CreateInstance<StyleSheet>();

        #region inspector
        
        [SerializeField]
        public StyleMap styles = new StyleMap();

        [SerializeField]
        public StyleSheet defaultStyle;
        
        #endregion

        public IReadOnlyDictionary<string, StyleSheet> Styles => styles;
        
        public StyleSheet GetStyle(string id)
        {
            if (styles.TryGetValue(id, out var styleSheet))
                return styleSheet;
            return defaultStyle ? defaultStyle : DefaultStyle;
        }
    }
}
