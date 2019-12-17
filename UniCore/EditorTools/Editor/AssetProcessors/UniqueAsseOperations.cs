namespace UniGreenModules.UniCore.EditorTools.Editor.AssetProcessors
{
    using System;
    using System.Collections.Generic;
    using AssetOperations;
    using Runtime.Interfaces;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class UniqueAssetsOperations
        
    {
        public static string[] OnWillSaveAssets<TAsset>(string[] paths)
            where TAsset : Object, IUnique
        {
            foreach (string path in paths) {
                var asset = AssetDatabase.LoadAssetAtPath<TAsset>(path);
                if (!asset) {
                    continue;
                }

                ProcessUniqueAssets<TAsset>();
                break;
            }
            return paths;
        }

        public static void ProcessUniqueAssets<TAsset>()
            where TAsset : Object, IUnique
        {
            var assetsMap = new HashSet<int>();
            var conflictedAssets = new List<TAsset>();
            
            var assets = AssetEditorTools.GetAssets<TAsset>();
            var maxId = Int32.MinValue;
            
            for (int i = 0; i < assets.Count; i++) {
                var asset = assets[i];
                var id = asset.Id;
                
                if (assetsMap.Contains(id)) {
                    conflictedAssets.Add(asset);
                    continue;
                }

                assetsMap.Add(id);
                
                if (id > maxId) maxId = id;
                
            }

            maxId = Math.Max(maxId, 0);
            
            for (int i = 0; i < conflictedAssets.Count; i++) {
                var asset = conflictedAssets[i];
                var id = asset.Id;
                
                asset.SetId(++maxId);
                
                Debug.LogError($"Found id conflict at {asset.name} old id: {id} new id:{asset.Id}");
            }
            
        }
    }
}
