namespace UnityAtlasGenerator.Editor.Helper
{
    using System;
    using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public class AtlasGeneratorAtlasSettingsOdinHandler : IDisposable
    {
        private AtlasGeneratorAtlasSettings _settings;
        private AtlasGeneratorAtlasEditorDrawer _atlasContainer;

        public void Initialize(AtlasGeneratorAtlasSettings target)
        {
            _settings = target;
            _atlasContainer = CreateDrawer(_settings);
        }

        public void Draw()
        {
            DrawInspectorTree();
            EditorUtility.SetDirty(_settings);
        }

        public void Dispose()
        {
            _settings = null;
            if (_atlasContainer)
            {
                Object.DestroyImmediate(_atlasContainer);
                _atlasContainer = null;
            }
        }

        private AtlasGeneratorAtlasEditorDrawer CreateDrawer(AtlasGeneratorAtlasSettings settings)
        {
            _atlasContainer = ScriptableObject.CreateInstance<AtlasGeneratorAtlasEditorDrawer>();
            _atlasContainer.Initialize(settings);
            return _atlasContainer;
        }

        private void DrawInspectorTree()
        {
            _atlasContainer?.Draw();
        }
    }

#else

    public class AtlasGeneratorAtlasSettingsOdinHandler : IDisposable
    {
        public void Initialize(AtlasGeneratorAtlasSettings target) { } 
        
        public void Draw() { }

        public void Dispose() { }
    }

#endif
}
