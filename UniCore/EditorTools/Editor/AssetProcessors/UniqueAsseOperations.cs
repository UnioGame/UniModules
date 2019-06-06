namespace UniGreenModules.UniCore.EditorTools
{
    using System;
    using System.Collections.Generic;
    using Runtime.Interfaces;
    using UniModule.UnityTools.EditorTools;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class UniqueAssetsOperations<TAsset>
        where TAsset : Object, IUnique
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (string path in paths) {
                var asset = AssetDatabase.LoadAssetAtPath<TAsset>(path);
                if (!asset) {
                    continue;
                }

                ProcessUniqueAssets();
                break;
            }
            return paths;
        }

        public static void ProcessUniqueAssets()
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
