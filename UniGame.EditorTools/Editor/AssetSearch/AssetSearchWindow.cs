#if ODIN_INSPECTOR

namespace UniModules.UniGame.EditorTools.Editor.AssetSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssetReferences;
    using Core.EditorTools.Editor.EditorResources;
    using Core.Runtime.Extension;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
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

        [Sirenix.OdinInspector.FolderPath]
        public List<string> searchFolders = new List<string>();

        public string searchFilter;
        
        public Object objectTypeFilter;

        public List<string> ignoreFilter = new List<string>();

        public List<string> referenceTypeFilter = AssetReferenceFinder.DefaultSearchTargets.ToList();

        public bool showEmptyReferences = false;
        
        [Space(16)]
        public List<ReferencesInfoData> references = new List<ReferencesInfoData>();

        #endregion

        private List<Type> _filterTypes = new List<Type>();
        
        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f,1,0.2f)]
        public void Search()
        {
            UpdateSearchResults();
        }

        [Sirenix.OdinInspector.Button]
        public void ResetFilters()
        {
            objectTypeFilter = null;
        }

        [Sirenix.OdinInspector.Button]
        public void ClearResults()
        {
            references.Clear();
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
                GetAssets<Object>(searchFilter, searchFolders.ToArray());
            //remove all filtered 
            assets.RemoveAll(x => FilterAsset(x) == false);

            var searchData = new SearchData() {
                assets = assets.ToArray(),
                excludeReferenceSearchFilters = ignoreFilter.ToArray(),
                referenceFilters = referenceTypeFilter.ToArray(),
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
                references.Add(referenceData);
            }
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


    }
}

#endif
