namespace UniGame.Core.EditorTools.Editor.UiElements
{
    using System;
    using Runtime.Attributes.FieldTypeDrawer;
    using UniModules.UniGame.UiElements.Editor;
    using UniModules.UniGame.UiElements.Editor.TypeDrawers;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    [UiElementsDrawer(100)]
    public class TextureDrawer : TexturePreviewDrawer<Texture>{}
    
    [UiElementsDrawer(100)]
    public class SpriteDrawer : TexturePreviewDrawer<Sprite>{}

    public class TexturePreviewDrawer<TAsset> : ObjectFieldViewDrawer<TAsset,TextureFieldView> 
        where TAsset : Object
    {
        protected override TextureFieldView CreateView(object source, Type type, string label = "")
        {
            var view = base.CreateView(source, type, label);
            view.RegisterValueChangedCallback(x => view.Texture = GetTexture(x.newValue));
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