using UnityEngine.UIElements;

namespace UniModules.UniGame.UiElements.Editor.Controls
{
    using System.Reflection;
    using UnityEngine.AddressableAssets;

    public class AssetReferenceFieldView : VisualElement
    {
        private readonly object target;
        
        private IMGUIContainer container;

        public FieldInfo FieldInfo { get; private set; }

        public AssetReference AssetReference { get; set; }

        public AssetReferenceFieldView(object target, FieldInfo info)
        {
            this.target = target;
            FieldInfo = info;
            
            container = new IMGUIContainer(Draw);
            Add(container);
        }

        private void Draw()
        {

        }
        
    }
}
