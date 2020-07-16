#if ODIN_INSPECTOR

namespace UniModules.UniGame.EditorTools.Editor.AssetSearchWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime.Extension;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetSearchWindow : OdinEditorWindow
    {
        #region statics
        
        [MenuItem("UniGame/Tools/Asset Search Window")]
        public static void Open()
        {
            var window = GetWindow<AssetSearchWindow>();
            window.Show();
        }
        
        #endregion
        
        #region inspector

        [Sirenix.OdinInspector.FolderPath]
        public List<string> searchFolders = new List<string>();

        //TODO Combine with one string filter search t:"" f:"" n:""
        public string searchFilter = String.Empty;

        public Object objectTypeFilter;

        [Sirenix.OdinInspector.InlineEditor]
        //[Sirenix.OdinInspector.AssetList(CustomFilterMethod = "FilterAsset")]
        [Sirenix.OdinInspector.PreviewField(70,Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
        public List<Object> resultAssets = new List<Object>();
        
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
            searchFilter = string.Empty;
            objectTypeFilter = null;
        }

        [Sirenix.OdinInspector.Button]
        public void MarkDirty()
        {
            foreach (var resultAsset in resultAssets) {
                resultAsset?.MarkDirty();
            }
        }

        [Sirenix.OdinInspector.Button]
        public void ClearResults()
        {
            resultAssets?.Clear();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }
        
        private void UpdateSearchResults()
        {
            resultAssets.Clear();
            _filterTypes.Clear();

            var filterType = Type.GetType(searchFilter, false, true);

            var assetType = objectTypeFilter is MonoScript scriptObject ?
                scriptObject.GetClass() :
                objectTypeFilter?.GetType();

            filterType?.AddToCollection(_filterTypes);
            assetType?.AddToCollection(_filterTypes);

            var assets = AssetEditorTools.GetAssets<Object>(searchFilter, searchFolders.ToArray());
            
            foreach (var asset in assets) {
                var result = FilterResultByTypes(asset, _filterTypes);
                if (result) {
                    resultAssets.Add(result);
                }
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
