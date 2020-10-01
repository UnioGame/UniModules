namespace UnityAtlasGenerator.Editor.Helper
{
    using System;
    using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public class AtlasGeneratorOdinHandler : IDisposable
    {
        private AtlasGeneratorSettings _settings;
        private AtlasGeneratorFilterOdinHandler _generationRulesContainer;
        private GUIContent _searchFieldLabel;
        private string _searchField;

        public void Initialize(AtlasGeneratorSettings target)
        {
            _settings = target;
            _generationRulesContainer = CreateDrawer(_settings);
        }

        public void Draw()
        {
            DrawInspectorTree(_searchField);

            EditorUtility.SetDirty(_settings);
        }

        public void Dispose()
        {
            _settings = null;
            if (_generationRulesContainer)
            {
                Object.DestroyImmediate(_generationRulesContainer);
                _generationRulesContainer = null;
            }
        }

        private AtlasGeneratorFilterOdinHandler CreateDrawer(AtlasGeneratorSettings settings)
        {
            _generationRulesContainer = ScriptableObject.CreateInstance<AtlasGeneratorFilterOdinHandler>();
            _generationRulesContainer.Initialize(settings);
            return _generationRulesContainer;
        }

        private void DrawInspectorTree(string filter)
        {
            _generationRulesContainer?.Draw();
        }
    }

#else

    public class AddressablesImporterOdinHandler : IDisposable
    {
        public void Initialize(AddressableImportSettings target) { } 
        
        public void Draw() { }

        public void Dispose() { }
    }

#endif
}
