namespace UniGame.LeoEcs.Debug.Editor
{
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class EntityViewWindow : OdinEditorWindow
    {
        #region statics data

        private static Color buttonColor = new Color(0.2f, 1, 0.6f);

        public static EntityViewWindow OpenWindow(EntityEditorView view)
        {
            var window = Create(view);
            window.Show();
            return window;
        }
    
        public static EntityViewWindow OpenPopupWindow(EntityEditorView view)
        {
            var window = Create(view);
            window.ShowPopup();
            return window;
        }

        public static EntityViewWindow Create(EntityEditorView view)
        {
            var window = GetWindow<EntityViewWindow>();
            window.titleContent.text = "Entities Debug View";
            window.entityView = view;
            return window;
        }

        #endregion

        #region inspector

        [HideLabel]
        [InlineProperty]
        public EntityEditorView entityView;

        #endregion

    }
}