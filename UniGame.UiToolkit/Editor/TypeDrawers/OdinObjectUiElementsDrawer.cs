using System;
using UniGame.Core.EditorTools.Editor.UiElements;
using UniGame.Core.Runtime.Attributes.FieldTypeDrawer;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniGame.UiElements.Editor.TypeDrawers
{
    using UniModules.UniGame.Core.EditorTools.Editor.DrawersTools;

#if ODIN_INSPECTOR
    [UiElementsDrawer(-1)]
#endif
    public class OdinObjectDrawer : IUiElementsTypeDrawer
    {
        public bool IsTypeSupported(Type type)
        {
            return typeof(object).IsAssignableFrom(type);
        }

        public VisualElement Draw(
            object source,
            Type type,
            string label = "",
            Action<object> onValueChanged = null)
        {
            var title = string.IsNullOrEmpty(label) ? "content" : 
                label;
            if (source == null) {
                title = $"{title} [EMPTY]";
            }
            var container = new Foldout() {
                text  = title,
                value = false,
                style = {
                    paddingLeft     = 4,
                    color           = new StyleColor(Color.black),
                    backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f))
                }
            };

#if ODIN_INSPECTOR
            container.Add(new IMGUIContainer(() => source.DrawOdinPropertyInspector()));
#endif

            return container;
        }
        
    }
}