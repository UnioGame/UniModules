namespace UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class OdinValueView
    {
        public System.Action<Object> AssetAction;
        public Object         Value;
        public bool           IsOpen;
        public string         Label;
        public bool           ShowFoldout;

        public VisualElement View;

        public OdinValueView()
        {
            View = new IMGUIContainer(() => {
                var target = Value;
                if (ShowFoldout) {
                    IsOpen = target.DrawOdinPropertyWithFoldout(IsOpen, Label, x => {
                        Value = x;
                        AssetAction?.Invoke(x);
                    });
                }
                else {
                    target.DrawOdinPropertyInspector();
                }
            });
        }
    }
}