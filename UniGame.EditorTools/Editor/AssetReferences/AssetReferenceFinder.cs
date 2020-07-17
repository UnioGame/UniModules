using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Linq;
    using Core.EditorTools.Editor.EditorResources;
    using Core.EditorTools.Editor.Tools;
    using Sirenix.Utilities;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using Object = Object;

    public class AssetReferenceFinder
    {
        public static IReadOnlyList<string> DefaultSearchTargets = new List<string>(){
            "*.prefab",
            "*.unity",
            "*.mat",
            "*.asset",
        };
        
        public static SearchResult FindReferences(SearchData searchData)
        {
            var result = LoadTargetAssets(searchData);

            if (result.referenceMap.Count == 0)
                return result;

            var searchTargets = SearchReferenceFiles(searchData.referenceFilters, searchData.excludeReferenceSearchFilters);

            result = UpdateReferences(result, searchTargets);

            return result;
            
        }

        public static SearchResult UpdateReferences(SearchResult result, HashSet<string> files)
        {
            foreach (var item in result.referenceMap) {
                item.Value.Clear();
            }

            AssetEditorTools.ShowActionProgress(FindAsssetReferences(result, files));
            
            return result;
        }

        public static IEnumerator<ProgressData> FindAsssetReferences(SearchResult result, HashSet<string> files)
        {
            var filesCount = files.Count;
            var allItems = filesCount;
            var progressIndex = 0;
            var referencesCount = 0;

            foreach (var file in files) {
                var localPath = EditorFileUtils.ToProjectPath(file);
                var fileValue = File.ReadAllText(file);
                                
                progressIndex++;

                foreach (var info in result.assetsInfos) {
                    var guid        = info.Value.guid;
                    var targetAsset = info.Value.asset;

                    yield return new ProgressData() {
                        Title    = localPath,
                        Progress = progressIndex/(float)allItems,
                        Content  = $"Found References: {referencesCount}"
                    };
                        
                    if (fileValue.IndexOf(guid, StringComparison.OrdinalIgnoreCase) < 0) 
                        continue;
                    var referenceItem = result.referenceMap[info.Key];
                    var asset         = localPath.AssetPathToAsset();
                    if(asset == targetAsset)
                        continue;
                    if(!asset) 
                        continue;
                            
                    referenceItem.Add(new EditorResource().Update(asset));
                    referencesCount++;
                }

            }
        }
        
        public static HashSet<string> SearchReferenceFiles(string[] filters,string[] excludeFilters = null)
        {
            var filterResult = new HashSet<string>();
            
            foreach (var filter in filters) {
                var paths = Directory.GetFiles(Application.dataPath, filter, SearchOption.AllDirectories);
                var assets = excludeFilters == null || excludeFilters.Length <=0 ? paths :
                    paths.Where(x => !excludeFilters.Any(y => x.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0));
                filterResult.AddRange(assets);
            }

            return filterResult;
        }

        public static SearchResult LoadTargetAssets(SearchData filter)
        {
            var result = new SearchResult();
            
            foreach (var guid in filter.assetsGuids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                result.AddKey(asset);
            }

            foreach (Object asset in filter.assets) {
                result.AddKey(asset);
            }

            if (filter.assetFolders.Length > 0) {
                AssetEditorTools.GetAssets<Object>(filter.assetFolders).ForEach(x => result.AddKey(x));
            }

            return result;
        }
  
    }
}
