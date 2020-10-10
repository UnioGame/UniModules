namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using System.Collections;
    using System.Linq;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.UiElements.Editor.TypeDrawers;
    using UnityEngine.UIElements;

    [UiElementsDrawer(900)]
    public class CollectionUiElementsDrawer : 
        UiElementsTypeDrawer
    {
        public override bool IsTypeSupported(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }

        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {
            var container = new VisualElement();
            
            if (!(source is IList list)) return container;
                
            var foldout = UiElementFactory.CreateFoldout($"[{label} : {list.Count}]");
            container.Add(foldout);

            var collectionType = list.GetType();
            var genericType = collectionType.
                GetGenericArguments().
                FirstOrDefault();

            var listView = new ListView(list,20, () => {
                    return new VisualElement();
                },
                (x, i) => {
                    var item       = list[i];
                    var targetType = genericType != null ? genericType : item?.GetType();
                    x.Add(UiElementFactory.Create(
                        list[i],targetType,
                        value => list[i] = value,
                        i.ToStringFromCache()));   
                });
            
            container.Add(listView);
            
            return container;
        }
    }
    
}