namespace UnityAtlasGenerator.Editor.Helper
{
#if ODIN_INSPECTOR

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.U2D;

    public class AtlasGeneratorAtlasEditorDrawer : ScriptableObject
    {
        private AtlasGeneratorAtlasSettings _atlasSettings;
        private PropertyTree _drawerTree;
        private bool _sourceChanged = false;

        [SerializeField]
        [AssetsOnly]
        private SpriteAtlas _defaultAtlas;

        public void Initialize(AtlasGeneratorAtlasSettings atlasSettings)
        {
            _atlasSettings = atlasSettings;
            _drawerTree = PropertyTree.Create(this);

            _drawerTree.OnPropertyValueChanged += (property, index) => EditorUtility.SetDirty(_atlasSettings);
        }

        public void Draw()
        {
            try
            {
                _drawerTree.Draw();
                ApplyChanges();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        [Button]
        public void Save() => _atlasSettings.Save();

        #region private methods

        private void ApplyChanges()
        {
            _drawerTree.ApplyChanges();
            _atlasSettings.DefaultAtlas = _defaultAtlas;

        }

        private void OnDisable()
        {
            if (_drawerTree == null) return;
            _drawerTree.OnPropertyValueChanged -= OnPropertyChanged;
            _drawerTree.Dispose();
        }

        private void OnPropertyChanged(InspectorProperty property, int index)
        {
            if (_atlasSettings == null) return;
            EditorUtility.SetDirty(_atlasSettings);
        }

        #endregion
    }
#endif
}