using UnityEditor;

namespace UniModules.UniGame.UiElements.Examples.Editor
{
    using System.Collections.Generic;
    using global::UniGame.Core.EditorTools.Editor.UiElements;
    using UiElements.Editor.TypeDrawers;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class DemoTypeDrawerWindow : EditorWindow
    {
        private VisualElement _root;
        private VisualElement _baseContainer;
        private List<object> _sourceAssets;
        
        public static DemoTypeDrawerWindow CreateWindow()
        {
            // Opens the window, otherwise focuses it if it’s already open.
            var window = GetWindow<DemoTypeDrawerWindow>();

            // Adds a title to the window.
            window.titleContent = new GUIContent("DemoTypeDrawer");

            // Sets a minimum size to the window.
            window.minSize = new Vector2(250, 250);

            return window;
        }

        public static void Show(List<object> demoAsset)
        {
            var window = CreateWindow();
            window.Initialize(demoAsset);
        }

        public void Initialize(List<object> demoTypeAsset)
        {
            _sourceAssets = demoTypeAsset;
            Refresh();
        }

        public void Refresh()
        {
            _root = rootVisualElement;
            if (_baseContainer != null) {
                _root.Remove(_baseContainer);
            }
            _baseContainer = null;
            Create();
        }

        private void Create()
        {
            _baseContainer = new VisualElement();
            _root.Add(_baseContainer);
            
            _baseContainer.Add(new Button(Refresh) {
                text = "Refresh",
            });
            
            var typeContainer = new VisualElement();
            _baseContainer.Add(typeContainer);

            foreach (var target in _sourceAssets) {
                var elementView = UiElementFactory.Create(target);
                typeContainer.Add(elementView);
            }

        }
        
        private void OnEnable()
        {
            _root = rootVisualElement;
        }
        
        
    }
}
