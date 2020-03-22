namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces.Rx;
    using UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public class UiElementValueDrawer 
    {

        private HashSet<object> shownValues;
        private Dictionary<Type,Func<object,VisualElement>> drawers;

        public UiElementValueDrawer()
        {
            shownValues = new HashSet<object>();
            drawers = CreateDrawers();
        }
        
        public VisualElement Create(object data)
        {
            shownValues.Clear();
            return CreateVisualElement(data);
        }
        
        public VisualElement CreateObjectView(Object target,string label = "")
        {
            if(target == null) return new ObjectField();

            var targetName = target.name;
            
            var field = new ObjectField(targetName) {
                objectType = target.GetType(),
                value = target,
                allowSceneObjects = true,
                visible = true,
            };
            if (string.IsNullOrEmpty(label) == false) {
                field.label = label;
            }
            
            return field;
            
        }
        
        public VisualElement CreateUnityObjectView(Object asset,string label = "")
        {
            if (asset == null) return null;
            var foldOut = new []{false};
            var element = new IMGUIContainer(() => {
                foldOut[0] = asset.DrawOdinPropertyWithFoldout(foldOut[0],label);
            }) 
            {
                style = {
                    minHeight = 20
                }
            };
            return element;
        }
        
        public VisualElement CreateSpriteView(Sprite sprite)
        {
            return sprite == null ? null : CreateTextureView(sprite.texture);
        }
        
        public VisualElement CreateTextureView(Texture asset)
        {
            if (asset == null) return null;
            
            var element = new Image() {
                image     = asset,
                name      = asset.name,
                scaleMode = ScaleMode.ScaleToFit,
                style = {
                    width  = 128,
                    height = 128,
                }
            };
            return element;
        }
        
        public VisualElement CreateClassView(object target)
        {
            if (target == null) return null;
            
            var type = target.GetType();
            if (type.IsValueType || target is string) {
                return new TextElement() {
                    text = target.ToString()
                };
            }
            
            var container = new VisualElement();
            var fields = type.GetFields(
                BindingFlags.Instance | 
                BindingFlags.Public | 
                BindingFlags.NonPublic |
                BindingFlags.Default);
            
            foreach (var field in fields) {
                var value = field.GetValue(target);
                var element = CreateVisualElement(value);
                if(element == null) continue;

                var foldout = CreateFoldout($"{field.Name} [{field.FieldType}]");
                element.name = field.Name;
                
                foldout.Add(element);
                container.Add(foldout);
            }

            return container;
        }
        
        public VisualElement CreateContainerView(ITypeData values)
        {
            if (values == null) return null;
            
            var container = new ListView();
            var foldOutItems = new ListView();

            foreach (var pair in values.EditorValues) {
                var valueContainer = pair.Value;
                var objectValue    = valueContainer as IObjectValue;
                var type           = pair.Key;

                if (valueContainer.HasValue == false || objectValue == null)
                    continue;

                var value     = objectValue.GetValue();
                var valueType = value?.GetType();

                var foldout = CreateFoldout($"[Registered Type :{type.Name}] : [Value Type :{valueType?.Name}]");

                foldout.Add(foldOutItems);
                container.Add(foldout);

                var element = CreateVisualElement(value);
                //is empty value or value already shown
                if (element == null)
                    continue;
  
                foldOutItems.Add(element);
               
            }
            
            return container;
        }
        
        public VisualElement CreateCollectionView(ICollection collection)
        {
            
            var container = new VisualElement();
            
            if (collection == null) return container;

            var index = 0;
            foreach (var value in collection) {
                
                var element = CreateVisualElement(value);
                //is empty value or value already shown
                if (element == null) continue;
                
                var label = $"Item {index++}";
                var foldout = CreateFoldout($"[{label} : {value.GetType().Name}]");
                
                container.Add(foldout);
                foldout.Add(element);
            }

            //container.style.minHeight = index * 20;
            
            return container;
        }
   
        #region private methods

        private Foldout CreateFoldout(string label, int marginLeft = 20)
        {
            var foldout = new Foldout() {
                text = label,
                style = {
                    marginLeft = marginLeft,
                }
            };
            return foldout;
        }
        
        private Dictionary<Type, Func<object,VisualElement>> CreateDrawers()
        {
            var drawers = new Dictionary<Type,Func<object,VisualElement>>() {
                { typeof(ITypeData), x => CreateContainerView(x as ITypeData)},
                { typeof(Sprite), x => CreateSpriteView(x as Sprite)},
                { typeof(Texture), x => CreateTextureView(x as Texture)},
                { typeof(ScriptableObject), x => CreateUnityObjectView(x as ScriptableObject)},
                { typeof(GameObject), x => CreateObjectView(x as GameObject)},
                { typeof(Object), x => CreateObjectView(x as Object)},
                { typeof(ICollection), x => CreateCollectionView(x as ICollection)},
                { typeof(object), x => CreateClassView(x)},
            };
            return drawers;
        }

        private VisualElement CreateVisualElement(object data)
        {
            if (data == null) return null;

            var type = data.GetType();
            if (type.IsClass && shownValues.Add(data) == false)
                return null;

            foreach (var drawer in drawers) {
                var drawerType = drawer.Key;
                if(drawerType.IsAssignableFrom(type) == false)
                    continue;
                var drawerFactory = drawer.Value;
                return drawerFactory(data);
            }

            return CreateClassView(data);
        }
        
        #endregion
    }
}
