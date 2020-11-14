using UnityEngine;

namespace UniModules.UniGame.UiElements.Editor
{
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    public class TextureFieldView : ObjectField
    {
        private Sprite _sprite;
        
        public Image Image { get; private set; }

        public Texture Texture {
            get => Image.image;
            set => Image.image = value;
        }
        
        public Sprite Sprite {
            get => _sprite;
            set {
                _sprite = value;
                Texture = _sprite.texture;
            }
        }

        public TextureFieldView()
        {
            Image = new Image() {
                name      = label,
                scaleMode = ScaleMode.ScaleToFit,
                style = {
                    width       = 32,
                    height      = 32,
                    marginRight = 4,
                    alignSelf   = new StyleEnum<Align>(Align.FlexEnd)
                }
            };
            Add(Image);
        }
        
    }
}
