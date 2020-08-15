#if ODIN_INSPECTOR

namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.EditorTools.Editor.AssetOperations.AssetReferenceTool;
    using Core.EditorTools.Editor.EditorResources;
    using Core.EditorTools.Editor.Tools;
    using Core.Runtime.Extension;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetSearchWindow : OdinEditorWindow
    {
        #region statics
        
        [MenuItem("UniGame/Tools/Asset Reference Search Window")]
        public static void Open()
        {
            var window = GetWindow<AssetSearchWindow>();
            window.Show();
        }
        
        #endregion
        
        #region inspector

        public string filter;
        
        [Tooltip("If target is MonoScript, when type is real target class type")]
        [Sirenix.OdinInspector.OnValueChanged("OnTargetAssetChanged")]
        public Object objectTypeFilter;

        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.InlineProperty]
        public ResourceHandle targetAsset;
        
        [Sirenix.OdinInspector.FolderPath]
        public List<string> folderFilter = new List<string>();
        
        [Space(6)]
        [Sirenix.OdinInspector.FoldoutGroup("Advanced")]
        public List<string> typeFilter = AssetReferenceFinder.DefaultSearchTargets.ToList();

        [Sirenix.OdinInspector.FoldoutGroup("Advanced")]
        [Space(8)]        
        public List<string> ignoreFilter = new List<string>();

        [Sirenix.OdinInspector.FoldoutGroup("Advanced")]
        [Sirenix.OdinInspector.FolderPath]
        public List<string> ignoreFolders = new List<string>();

        [Sirenix.OdinInspector.OnValueChanged("FilterReferences")]
        public bool showEmptyReferences = false;
        
        [Space(16)]
        public List<ReferencesInfoData> references = new List<ReferencesInfoData>();

        #endregion

        [HideInInspector]
        [SerializeField]
        private List<ReferencesInfoData> _referencesData = new List<ReferencesInfoData>();
        
        private List<Type> _filterTypes = new List<Type>();
        
        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f,1,0.2f)]
        public void Search()
        {
            UpdateSearchResults();
        }

        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f,1,0.2f)]
        public void ClearResults()
        {
            references.Clear();
            _referencesData.Clear();
        }

        private void UpdateSearchResults()
        {
            ClearResults();
            
            _filterTypes.Clear();

            var assetType = objectTypeFilter is MonoScript scriptObject ?
                scriptObject.GetClass() :
                objectTypeFilter?.GetType();

            assetType?.AddToCollection(_filterTypes);

            var assets = AssetEditorTools.
                GetAssets<Object>(filter, folderFilter.ToArray());
            //remove all filtered 
            assets.RemoveAll(x => FilterAsset(x) == false);

            var searchData = new SearchData() {
                assets = assets.ToArray(),
                regExFilters = ignoreFilter.
                    Concat(ignoreFolders).
                    Select(EditorFileUtils.FixUnityPath).
                    ToArray(),
                fileTypes = typeFilter.ToArray(),
            };

            var result = AssetReferenceFinder.FindReferences(searchData);
            
            foreach (var reference in result.referenceMap) {
                var assetItem = reference.Key;
                var referencesData = reference.Value.
                    Select(x => x.asset).
                    ToList();
                var referenceData = new ReferencesInfoData() {
                    source     = new EditorResource().Update(assetItem),
                    references = referencesData
                };
                _referencesData.Add(referenceData);
            }

            FilterReferences();
        }

        private bool FilterAsset(Object asset)
        {
            var filteredAsset = FilterResultByTypes(asset, _filterTypes);
            return filteredAsset!=null;
        }
        
        private Object FilterResultByTypes(Object source,List<Type> types)
        {
            if (!source || types == null || types.Count == 0)
                return source;
            
            var assetPath = AssetDatabase.GetAssetPath(source);
            
            var gameObject = source as GameObject;
            if (!gameObject && types.Any(x => x.IsInstanceOfType(source))) {
                return source;
            }
            
            var importer     = AssetImporter.GetAtPath(assetPath);
            var importerType = importer.GetType();
            foreach (var type in types) {
                if (type.IsAssignableFrom(importerType))
                    return importer;
            }

            if (!gameObject) {
                return null;
            }

            foreach (var assetType in types) {
                if(!assetType.IsComponent()) 
                    continue;
                var target = gameObject.GetComponent(assetType);
                if (target) {
                    return target;
                }
            }

            return null;
        }


        private Object OnTargetAssetChanged()
        {
            targetAsset = new EditorResource().Update(objectTypeFilter);
            return objectTypeFilter;
        }
        
        private void FilterReferences()
        {
            references.Clear();
            if (showEmptyReferences) {
                references.AddRange(_referencesData.Where(x => x.references.Count == 0));
                return;
            }
            references.AddRange(_referencesData);
        }

    }
}

#endif
