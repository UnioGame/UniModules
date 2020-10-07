#if ODIN_INSPECTOR

using Sirenix.OdinInspector.Editor;

namespace UniModules.UniGame.UiElements.Editor.Windows
{
    using System;
    using System.Collections.Generic;
    using Core.EditorTools.Editor.AssetOperations;
    using global::UniGame.Core.EditorTools.Editor.UiElements;
    using Sirenix.OdinInspector;
    using TypeDrawers;
    using UniModules.UniCore.EditorTools.Editor.Utility;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class DrawerItemInfo
    {
        public string name;
        public Type type;
        public MonoScript source;

        [Button]
        public void OpenScript()
        {
            type?.OpenEditorScript();
        }
    }
    
    public class UiElementsDrawersWindow : OdinEditorWindow
    {
        [MenuItem("UniGame/UiElements/Drawers")]
        private static void OpenWindow()
        {
            var window = GetWindow<UiElementsDrawersWindow>();
            window.InitializeWindow();
            window.Show();
        }
        
        private LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();
        
        [SerializeField]
        private List<DrawerItemInfo> _drawers = new List<DrawerItemInfo>();
        [Space]
        [SerializeField]
        private List<DrawerItemInfo> _fieldDrawers = new List<DrawerItemInfo>();
        
        public void InitializeWindow()
        {
            _lifeTimeDefinition.Release();
            UiElementFactory.
                Ready.
                Subscribe(x => UpdateView()).
                AddTo(_lifeTimeDefinition);
        }

        private void UpdateView()
        {
            foreach (var drawer in UiElementFactory.Drawers) {
                var type = drawer.GetType();
                _drawers.Add(new DrawerItemInfo() {
                    type = type,
                    name = type.Name,
                    source = AssetEditorTools.GetScriptAsset(type),
                });
            }
            
            foreach (var drawer in UiElementFactory.FieldDrawers) {
                var type = drawer.GetType();
                _drawers.Add(new DrawerItemInfo() {
                    type   = type,
                    name   = type.Name,
                    source = AssetEditorTools.GetScriptAsset(type),
                });
            }
        }

        protected override void OnDestroy()
        {
            _lifeTimeDefinition.Terminate();
            base.OnDestroy();
        }
    }
}

#endif