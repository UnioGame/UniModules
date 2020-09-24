namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UniModules.UniGame.UiElements.Editor.TypeDrawers;
    using UnityEngine.UIElements;

    public class ClassUiElementsDrawer : UiElementsTypeDrawer
    {

        public override bool IsTypeSupported(Type type)
        {
            return true;
        }

        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {
            var fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance | 
                BindingFlags.Default);

            var visibleFields = fields.
                Where(UiElementFactory.IsVisibleField).
                ToList();
            
            var container = new VisualElement();
            
            foreach (var field in visibleFields) {

                var fieldContainer = container;
                var value = field.GetValue(source);
                
                var element = UiElementFactory.Create(
                    value,
                    field.FieldType,
                    x => field.SetValue(source,x),
                    field.Name);
                
                if(element == null) continue;

                if (IsFoldoutObject(field.FieldType)) {
                    var foldout = UiElementFactory.CreateFoldout(string.IsNullOrEmpty(label) ? field.Name : label);
                    fieldContainer.Add(foldout);
                    fieldContainer = foldout;
                }
                
                fieldContainer.Add(element);
            }

            return container;
        }

        private bool IsFoldoutObject(Type type)
        {
            if (type.IsValueType)
                return false;

            if (typeof(IList).IsAssignableFrom(type))
                return true;
            
            if (UiElementFactory.UnityObjectType.IsAssignableFrom(type))
                return false;
            
            if (type == typeof(string)) return false;

            return true;
        }
    }
}