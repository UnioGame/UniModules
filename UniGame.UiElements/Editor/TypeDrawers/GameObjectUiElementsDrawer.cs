namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniModules.UniGame.Core.EditorTools.Editor.DrawersTools;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    //[UiElementsDrawer(100)]
    public class GameObjectUiElementsDrawer : UiElementsTypeDrawer
    {
        public override bool IsTypeSupported(Type type)
        {
            return typeof(GameObject).IsAssignableFrom(type) || 
                   typeof(Component).IsAssignableFrom(type);
        }

        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {
            var asset = source as Object;

            var field = new IMGUIContainer(() => {
                asset = asset.DrawOdinPropertyField(type,onValueChanged,true,label);
            });
            
            return field;
        }
    }
}