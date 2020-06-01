using UnityEngine;

namespace UniModules.UniGame.UiElements.Examples.Editor
{
    using System.Collections.Generic;

    [CreateAssetMenu(menuName = "UniGame/UiElements/Examples/DemoTypeAsset",fileName = "DemoTypeAsset")]
    public class DemoTypeAsset : ScriptableObject
    {
        public List<Object> assets;

        public Component ComponentValue;
        public GameObject GameObjectValue;
        public Vector3 Vector3Value = Vector3.right;
        public Vector2 Vector2Value = Vector2.left;
        public Sprite SpriteValue;
        public Texture TextureValue;
        public int IntValue;
        public bool BoolValue;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        [ContextMenu("Show UniElements Demo Window")]
        public void Show()
        {
            var targets = new List<object>() {
                this,
                GameObjectValue,
                ComponentValue,
                // assets,
                // Vector2Value,
                // Vector2Value,
                // SpriteValue,
                // TextureValue,
                // IntValue,
                // BoolValue,
            };
            DemoTypeDrawerWindow.Show(targets);
        }
    
    
    }
}
