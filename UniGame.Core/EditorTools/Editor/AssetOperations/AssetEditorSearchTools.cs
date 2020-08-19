namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public partial class AssetEditorTools
    {
        #region asset loading

        private static string[] EmptyDirFilter = new string[0];
        
        public static List<Object> GetAssets(Type assetType, string[] folders = null)
        {
            var filterText = $"t:{assetType.Name}";
            var assets     = GetAssets<Object>(filterText, folders);
            return assets;
        }

        public static List<Object> FindAssets(List<Object> assets,Type assetType,string[] folders)
        {
            var filterText = $"t:{assetType.Name}";

            var ids =  AssetDatabase.FindAssets(filterText, folders);
            
            for (var i = 0; i < ids.Length; i++) {
                var id        = ids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath)) {
                    Debug.LogErrorFormat("Asset importer {0} with NULL path detected", id);
                    continue;
                }
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);
                if (asset) assets.Add(asset);
            }

            return assets;
        }

        public static Object GetAsset(string filter, string[] folders = null)
        {
            return GetAssets(filter, folders).FirstOrDefault();
        }
        
        public static List<Object> GetAssets(string filter,Type type, string[] folders = null)
        {
            return GetAssets(filter, folders).
                Where(x => x && type.IsInstanceOfType(x)).
                ToList();
        }
        
        public static List<Object> GetAssets(string filter, string[] folders = null)
        {
            if (string.IsNullOrEmpty(filter))
                return null;
            
            var path = AssetDatabase.GUIDToAssetPath(filter);
            return !string.IsNullOrEmpty(path) ? 
                new List<Object>(){AssetDatabase.LoadAssetAtPath<Object>(path)} : 
                GetAssets<Object>(filter, folders);
        }

        public static List<T> GetAssets<T>(string filter, string[] folders = null) where T : Object
        {
            var assets = GetAssets(new List<T>(), filter, folders);
            return assets;
        }

        public static List<T> GetAssetsByPaths<T>(List<string> paths) where T : Object
        {
            var assets = new List<T>();
            for (var i = 0; i < paths.Count; i++) {
                var path  = paths[i];
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (!asset) continue;
                assets.Add(asset);
            }

            return assets;
        }

        public static List<T> GetAssets<T>(List<T> resultContainer, string filter, string[] folders = null) where T : Object
        {
            var type = typeof(T);
            var ids  = AssetDatabase.FindAssets(filter,folders ?? EmptyDirFilter);
            for (var i = 0; i < ids.Length; i++) {
                var id        = ids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath)) {
                    Debug.LogErrorFormat("Asset importer {0} with NULL path detected", id);
                    continue;
                }

                T asset = null;

                asset = AssetDatabase.LoadAssetAtPath(assetPath, type) as T;

                if (asset) resultContainer.Add(asset);
            }

            return resultContainer;
        }
 
        /// <summary>
        /// load components
        /// </summary>
        /// <param name="type">component type</param>
        /// <param name="folders">folder filter</param>
        /// <returns>list of found items</returns>
        public static List<Object> GetComponentsAssets(Type type, string[] folders = null)
        {

            if (IsComponent(type) == false) return new List<Object>();

            var filterText   = string.Format("t:{0}", typeof(GameObject));
            var assets       = GetAssets<GameObject>(filterText, folders);
            var resultAssets = new List<Object>();

            for (int i = 0; i < assets.Count; i++)
            {
                var targetComponents = assets[i].GetComponents(type);
                resultAssets.AddRange(targetComponents);
            }

            return resultAssets;

        }


        #endregion
    }
}