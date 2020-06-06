namespace UniGreenModules.UniGame.Core.EditorTools.Editor.DrawersTools
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class OdinFieldView
    {
        public SerializedProperty Property;
        public bool               IsOpen;
        public bool               IncludeChildren = true;
        public GUIContent         Label;

        public VisualElement View;

        public OdinFieldView()
        {
            View = new IMGUIContainer(() => {
                var target = Property;
                IsOpen = target.OdinFieldFoldout(IsOpen, Label, IncludeChildren);
            });
        }
    }
}