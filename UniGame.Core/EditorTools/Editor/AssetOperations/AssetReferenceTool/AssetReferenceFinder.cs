namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations.AssetReferenceTool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using EditorResources;
    using Sirenix.Utilities;
    using Tools;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetReferenceFinder
    {
        public static IReadOnlyList<string> DefaultSearchTargets = new List<string>(){
            "*.prefab",
            "*.unity",
            "*.mat",
            "*.asset",
        };

        public static SearchResultData FindReferences(SearchData searchData)
        {
            var result = LoadTargetAssets(searchData);

            if (result.referenceMap.Count == 0)
                return result;

            var searchTargets = SearchReferenceFiles(searchData.referenceFilters, searchData.excludeReferenceSearchFilters);

            result = UpdateReferences(result, searchTargets);

            return result;
            
        }

        public static SearchResultData UpdateReferences(SearchResultData searchResult, HashSet<string> files)
        {
            foreach (var item in searchResult.referenceMap) {
                item.Value.Clear();
            }

            AssetEditorTools.ShowActionProgress(searchResult.Progression,searchResult.LifeTime).
                AddTo(searchResult.LifeTime);
            searchResult = FindAsssetReferences(searchResult, files);
            searchResult.Complete();
            return searchResult;
        }

        public static SearchResultData FindAsssetReferences(SearchResultData searchData, HashSet<string> files)
        {
            var lifetime = searchData.LifeTime;
            var filesCount = files.Count;
            var allItems = filesCount;
            var progressIndex = 0;
            var referencesCount = 0;
            var cancelationToken = lifetime.AsCancellationSource().Token;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            files.AsParallel().
                WithCancellation(cancelationToken).
                AsUnordered().
                ForEach(file => {
                                    
                    Interlocked.Increment(ref progressIndex);

                    if (Directory.Exists(file))
                        return;
                    
                    var localPath = EditorFileUtils.ToProjectPath(file);
                    var fileValue = File.ReadAllText(file);
                    var assets = searchData.assets;

                    assets.AsParallel().WithCancellation(cancelationToken).AsUnordered().Select(x => x.Value).ForEach(x => {
                        var guid        = x.guid;
                        var targetAsset = x.asset;

                        var timespan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

                        searchData.ReportProgress(new ProgressData() {
                            Title    = localPath,
                            Progress = progressIndex / (float) allItems,
                            Content  = $"Found References: {referencesCount} : time {timespan.TotalSeconds}..."
                        });

                        if (fileValue.IndexOf(guid, StringComparison.OrdinalIgnoreCase) < 0)
                            return;
                        var referenceItem = searchData.referenceMap[targetAsset];
                        var asset         = localPath.AssetPathToAsset();
                        if (asset == targetAsset)
                            return;
                        if (!asset)
                            return;

                        referenceItem.Add(new EditorResource().Update(asset));
                        Interlocked.Increment(ref referencesCount);
                    });

                });
            
            stopwatch.Stop();
            return searchData;
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

        public static SearchResultData LoadTargetAssets(SearchData filter)
        {
            var result = new SearchResultData();
            
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
