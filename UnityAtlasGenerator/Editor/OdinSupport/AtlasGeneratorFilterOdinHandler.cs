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

    public class AtlasGeneratorFilterOdinHandler : ScriptableObject
    {
        private AtlasGeneratorSettings _generationSettings;
        private PropertyTree _drawerTree;
        private List<Func<AtlasGeneratorRule, string, bool>> _filters;
        private bool _sourceChanged = false;

        [SerializeField]
        [HideLabel]
        [OnValueChanged("OnFilterChanged")]
        private string _searchFilter;

        [SerializeField]
        [ListDrawerSettings(
            HideRemoveButton = true,
            Expanded = true,
            CustomAddFunction = nameof(CustomAddFunction),
            OnEndListElementGUI = nameof(EndOfListItemGui),
            CustomRemoveElementFunction = nameof(CustomRemoveElementFunction),
            CustomRemoveIndexFunction = nameof(CustomRemoveIndexFunction),
            ShowPaging = true
        )]
        private List<AtlasGeneratorRule> rules = new List<AtlasGeneratorRule>();

        public void Initialize(AtlasGeneratorSettings generationSettings)
        {
            _generationSettings = generationSettings;
            _drawerTree = PropertyTree.Create(this);

            _filters = new List<Func<AtlasGeneratorRule, string, bool>>() {
                ValidateAtlasName,
                ValidateRulePath,
            };

            _drawerTree.OnPropertyValueChanged += (property, index) => EditorUtility.SetDirty(_generationSettings);
        }

        public void Draw()
        {
            try
            {
                FilterRules(_searchFilter);
                _drawerTree.Draw();
                ApplyChanges();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        [Button]
        public void Save() => _generationSettings.Save();

        #region private methods

        private void OnFilterChanged()
        {

        }

        private bool ValidateRule(AtlasGeneratorRule rule, string filter)
        {
            return string.IsNullOrEmpty(filter) || _filters.Any(x => x(rule, filter));
        }

        private bool ValidateAtlasName(AtlasGeneratorRule rule, string filter)
        {
            return rule.pathToAtlas.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ValidateRulePath(AtlasGeneratorRule rule, string filter)
        {
            return rule.path.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void FilterRules(string filter)
        {
            rules = new List<AtlasGeneratorRule>();
            var filteredRules = _generationSettings.rules.
                Where(x => ValidateRule(x, filter));
            rules.AddRange(filteredRules);
        }

        private void ApplyChanges()
        {
            _drawerTree.ApplyChanges();

            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                var index = _generationSettings.rules.IndexOf(rule);
                if (index < 0) continue;
                _generationSettings.rules[index] = rules[i];
            }

        }

        private void CustomAddFunction()
        {
            _generationSettings.rules.Add(new AtlasGeneratorRule());
            _sourceChanged = true;
        }

        private void CustomRemoveIndexFunction(int index)
        {
            var removeResult = _generationSettings.rules.Remove(rules[index]);
            _sourceChanged = true;
        }

        private void CustomRemoveElementFunction(AtlasGeneratorRule item)
        {
            var index = rules.IndexOf(item);
            CustomRemoveIndexFunction(index);
        }

        private void EndOfListItemGui(int item)
        {
            if (GUILayout.Button("remove"))
            {
                CustomRemoveIndexFunction(item);
            }
        }

        private void OnDisable()
        {
            if (_drawerTree == null) return;
            _drawerTree.OnPropertyValueChanged -= OnPropertyChanged;
            _drawerTree.Dispose();
        }

        private void OnPropertyChanged(InspectorProperty property, int index)
        {
            if (_generationSettings == null) return;
            EditorUtility.SetDirty(_generationSettings);
        }

        #endregion
    }
#endif
}