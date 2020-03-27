namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    [UiElementsDrawer(100)]
    public class TextureDrawer : TexturePreviewDrawer<Texture>{}
    
    [UiElementsDrawer(100)]
    public class SpriteDrawer : TexturePreviewDrawer<Sprite>{}

    public class TexturePreviewDrawer<TAsset> : ObjectFieldDrawer<TAsset>
        where TAsset : Object
    {
        public override VisualElement Draw(object source, Type type, string label = "", Action<object> onValueChanged = null)
        {
            //var container = new VisualElement() {};
            
            var asset = GetTexture(source as Object);

            var image = new Image() {
                image     = asset,
                name      = label,
                scaleMode = ScaleMode.ScaleToFit,
                style = {
                    width  = 32,
                    height = 32,
                    marginRight = 4,
                    alignSelf = new StyleEnum<Align>(Align.FlexEnd)
                }
            };
            
            var view = base.Draw(source, type, label, x => {
                image.image = GetTexture(x as Object);
                onValueChanged?.Invoke(x);
            });
            
            //container.Add(view);
            view.Add(image);
            
            return view;
        }
        
        protected virtual Texture GetTexture(Object objectSource)
        {
            if (!objectSource)
                return UiElementFactory.DefaultTexture;
                
            var result = objectSource is Texture texture ? texture :
                objectSource is Sprite sprite ? sprite.texture :
                UiElementFactory.DefaultTexture;
            return result;
        }
    }
}