namespace UniGreenModules.UniCore.EditorTools.Editor.AssetOperations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Runtime.ReflectionUtils;
    using UnityEditor;
    using UnityEngine;
    using Utility;
    using Object = UnityEngine.Object;

    public partial class AssetEditorTools
    {
        private const string PrefabExtension = "prefab";
        private const string AssetExtension = "asset";
        
        private static Type _componentType = typeof(Component);
        private static List<Object> _emptyAssets = new List<Object>();
        private static string clientDataPath;
        private static Regex modificationRegex;

        public const string AssetRoot = "assets";
        public const string ModificationTemplate = @"\n( *)(m_Modifications:[\w,\W]*)(?=\n( *)m_RemovedComponents)";
        public static List<string> _modificationsIgnoreList = new List<string>() { ".fbx" };


        public static string GetAssetExtension(Object asset)
        {
            if (asset is GameObject)
                return PrefabExtension;
            if (asset is ScriptableObject)
                return AssetExtension;
            return string.Empty;
        }
        
        public static void ApplyProgressAssetAction<T>(List<T> assets, string message, Action<T> action)
        {

            var count = assets.Count;
            for (int i = 0; i < count; i++)
            {

                var asset = assets[i];
                if (asset == null) continue;
                var progress = i / ((float)count);
                EditorUtility.DisplayProgressBar(string.Format("Progress [{0} of {1}] :", i, count), asset.ToString(), progress);
                action(asset);

            }
            
        }

        public static bool OpenScript<T>(params string[] folders)
        {
            return OpenScript(typeof(T),folders);
        }

        public static bool OpenScript(Type type,params string[] folders)
        {
            var typeName  = type.Name;
            var filter    = $"t:script {typeName}";
            
            var assets = AssetDatabase.FindAssets(filter, folders);
            var assetGuid = assets.FirstOrDefault(
                    x => string.Equals(typeName,
                        Path.GetFileNameWithoutExtension(x.AssetGuidToPath()),
                        StringComparison.OrdinalIgnoreCase));
            
            if (string.IsNullOrEmpty(assetGuid))
                return false;
            
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetGuid.AssetGuidToPath());
            if (asset == null)
                return false;
            return AssetDatabase.OpenAsset(asset.GetInstanceID(), 0, 0);
        }
        
        public static void FindItems<T>(Action<Object, T> action)
        {

            var assets = AssetEditorTools.GetAllAssets();

            AssetEditorTools.ApplyTypeItemAction<T>(assets, action);

        }

        public static void ApplyTypeItemAction<TData>(List<Object> resources, Action<Object, TData> action, HashSet<object> excludedItems = null)
        {

            if (resources == null || resources.Count == 0) return;

            var items = new List<Object>();
            excludedItems = excludedItems != null ? excludedItems : new HashSet<object>();
            for (int i = 0; i < resources.Count; i++)
            {
                var asset = resources[i];
                if (asset == null) continue;

                GetAssets(asset, items);
                ProceedTypeAssets(items, action, excludedItems);

                var progress = i / ((float)resources.Count);
                var canceled = EditorUtility.DisplayCancelableProgressBar(string.Format("Prepare Assets [{0} of {1}] :", i, resources.Count), asset.name, progress);
                if (canceled)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public static void ApplyTypeItemAction<TData>(List<string> resources, Action<Object, TData> action) where TData : class
        {

            if (resources == null || resources.Count == 0) return;

            var items = new List<Object>();
            var excludedItems = new HashSet<object>();
            for (int i = 0; i < resources.Count; i++)
            {
                var assetPath = resources[i];
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset == null)
                {
                    Debug.LogErrorFormat("ApplyTypeItemAction load NULL asset at path {0}", assetPath);
                    continue;
                }
                GetAssets(asset, items);
                var progress = i / ((float)resources.Count);
                var canceled = EditorUtility.DisplayCancelableProgressBar(string.Format("Prepare Assets [{0} of {1}] : ", i, resources.Count), assetPath, progress);

                ProceedTypeAssets(items, action, excludedItems);
                if (canceled)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public static void ProceedTypeAssets<TData>(List<Object> assets, Action<Object, TData> action, HashSet<object> excludedItems = null)
        {
            GUI.changed = true;
            for (int j = 0; j < assets.Count; j++)
            {
                var asset = assets[j];
                if (!asset) continue;
                FindTypeItemsInAsset<TData>(asset, action, excludedItems);
            }
        }

        public static void FindTypeItemsInAsset<TData>(Object asset, Action<Object, TData> assetAction = null, HashSet<object> excludedItems = null,
                                                       Func<TData, TData> editAction = null)
        {
            try
            {
                ReflectionTools.FindResource<TData>(asset, assetAction, excludedItems, editAction);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void EditTypeItemsInAsset<TData>(Object asset, Func<TData, TData> editAction = null, HashSet<object> excludedItems = null)
        {
            try
            {
                ReflectionTools.FindResource<TData>(asset, null, excludedItems, editAction);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void SearchTypeFromAsset<TData>(Object asset, Action<Object, TData> assetAction, HashSet<object> excludedItems = null)
        {
            try
            {
                var items = new List<Object>();
                GetAssets(asset, items);
                excludedItems = excludedItems == null ? new HashSet<object>() : excludedItems;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    try
                    {
                        ReflectionTools.FindResource<TData>(item, assetAction, excludedItems);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static Dictionary<Object, List<TData>> SearchTypeFromAsset<TData>(Object asset, HashSet<object> excludedItems = null)
        {
            var result = new Dictionary<Object, List<TData>>();
            SearchTypeFromAsset<TData>(asset, (x, y) =>
            {

                List<TData> items = null;
                if (result.TryGetValue(x, out items))
                {
                    items.Add(y);
                }
                result[x] = new List<TData>() { y };

            });
            return result;
        }

        public static List<T> GetRootAssetsFiltered<T>() where T : Object
        {
            var items = new List<T>();
            var gameObjects = AssetEditorTools.GetAssets<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                var components = gameObjects[i].GetComponents<T>();
                if (components == null || components.Length == 0) continue;
                items.AddRange(components);
            }

            var assets = AssetEditorTools.GetAssets<T>();
            items.AddRange(assets);
            return items;
        }

        public static List<Object> GetAllAssets()
        {
            var items       = new List<Object>();
            var gameObjects = AssetEditorTools.GetAssets<GameObject>();
            items.AddRange(gameObjects.ToArray());

            var scriptableObjects = AssetEditorTools.GetAssets<ScriptableObject>().ToArray();
            items.AddRange(scriptableObjects);
            return items;
        }

        public static T GetAsset<T>(string[] folders = null) where T : Object
        {
            var asset = GetAssets<T>(folders).FirstOrDefault();
            return asset;
        }

        public static List<Object> GetAssets(Object asset, List<Object> output)
        {

            output.Clear();

            if (!asset) return output;

            var gameObjectComponent = asset as GameObject;
            if (gameObjectComponent)
            {
                output.AddRange(gameObjectComponent.GetComponentsInChildren<Component>(true));
            }
            output.Add(asset);

            return output;
        }

        public static List<EditorAssetResource> GetEditorResources<TSource>(string[] folders = null)
            where  TSource : Object
        {
            var result = AssetEditorTools.GetAssetsPaths<TSource>(folders).
                Select(x => {
                    var asset = new EditorAssetResource();
                    asset.Initialize(x);
                    return asset;
                }).ToList();
            return result;
        }

        public static List<T> GetAssets<T>(string[] folders = null) where T : Object
        {
            
            var targetType = typeof(T);
            return GetAssets<T>(targetType, folders);

        }
        
        public static List<T> GetAssets<T>(Type targetType,string[] folders = null) where T : Object
        {
            if (IsComponent(targetType))
            {
                var components = GetComponentAssets<T>(folders);
                return components;
            }
            var items = GetAssets(targetType, folders);
            return items.OfType<T>().ToList();
        }

        public static List<T> GetComponentAssets<T>(string[] folders = null) where T : Object
        {

            var result = new List<T>();
            ShowActionProgress(GetComponentAssets(result, folders));
            return result;

        }

        public static IEnumerator<ProgressData> GetComponentAssets<T>(List<T> container, string[] folders = null)
        {

            var progress = new ProgressData()
            {
                Content = "Loading...",
                Title = "GetComponentAssets " + typeof(T).Name,
            };
            yield return progress;
            var items = GetAssets<GameObject>(folders);
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var targetComponents = item.GetComponents<T>();
                container.AddRange(targetComponents);
                progress.Progress = (float)i / items.Count;
            }

        }

        public static List<T> GetAssetsWithChilds<T>(string[] folders = null) where T : Object
        {
            var childs = ReflectionTools.FindAllImplementations(typeof(T));
            var items = GetAssets(typeof(T), folders);
            for (int i = 0; i < childs.Count; i++)
            {
                var child = childs[i];
                var assets = GetAssets(child, folders);
                items.AddRange(assets);
            }
            return items.OfType<T>().ToList();
        }

        public static string GetFoldersTemplateName(string path, int depth, bool includeFileName = true, int startDepth = 0)
        {
            var result = includeFileName ?
                Path.GetFileNameWithoutExtension(path) : string.Empty;
            var tempPath = path;
            var pathLength = depth + startDepth;
            for (var i = 0; i < pathLength; i++)
            {
                var parent = Directory.GetParent(tempPath);
                if (parent == null)
                    break;
                var parentName = parent.Name;
                if (i >= startDepth)
                {
                    result = string.IsNullOrEmpty(result) ? parentName : string.Format("{0}-{1}", parentName, result);
                }
                tempPath = parent.FullName;
                if (string.Equals(parentName, AssetRoot, StringComparison.OrdinalIgnoreCase))
                    break;
            }

            result = GetValidBundleTag(result);

            return result;
        }
        
        public static void ShowActionProgress(IEnumerator<ProgressData> awaiter)
        {

            var isCanceled = EditorUtility.DisplayCancelableProgressBar(string.Empty, string.Empty, 0);

            while (isCanceled == false && awaiter.MoveNext())
            {

                var progress = awaiter.Current;
                isCanceled = EditorUtility.DisplayCancelableProgressBar(progress.Title, progress.Content, progress.Progress);
                if (isCanceled)
                    break;
            }

            EditorUtility.ClearProgressBar();

        }

        public static string GetUniqueAssetName(string path)
        {
            return AssetDatabase.GenerateUniqueAssetPath(path);
        }

        public static bool IsComponent(Type targetType)
        {
            return _componentType.IsAssignableFrom(targetType);
        }

                
        public static bool IsContainsDuplicatedAssets(List<AssetImporter> assets)
        {
            var assetsNames = assets.Select(x => Path.GetFileNameWithoutExtension(x.assetPath)).ToList();
            var groups      = assetsNames.GroupBy(x => x).ToList();
            return groups.Count != assets.Count;
        }

    }

}
