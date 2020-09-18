namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using EditorResources;
    using Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UniRx;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static partial class AssetEditorTools
    {
        private const string PrefabExtension = "prefab";
        private const string AssetExtension = "asset";
        
        private static Type _componentType = typeof(Component);
        private static List<Object> _emptyAssets = new List<Object>();
        private static string clientDataPath;
        private static Regex modificationRegex;

        private static Func<Object, EditorResource> _editorResourceFactory = MemorizeTool.Create<Object, EditorResource>(x => {
            var result = new EditorResource();
            if (!x) return result;
            result.Update(x);
            return result;
        });

        public const string AssetRoot = "assets";
        public const string ModificationTemplate = @"\n( *)(m_Modifications:[\w,\W]*)(?=\n( *)m_RemovedComponents)";
        public static List<string> _modificationsIgnoreList = new List<string>() { ".fbx" };

        
        public static bool IsPureEditorMode => 
            EditorApplication.isPlaying == false &&
            EditorApplication.isPlayingOrWillChangePlaymode == false && 
            EditorApplication.isCompiling == false && 
            EditorApplication.isUpdating == false;
        
        /// <summary>
        //	Can create Scriptable object/ component /gameobject
        /// </summary>
        public static Object CreateAsset(this Type type)
        {
            Object asset = null;
            switch (type) {
                case Type t when t.IsScriptableObject():
                    asset = ScriptableObject.CreateInstance(type);
                    break;
                case Type t when t.IsGameObject():
                    var gameObject = new GameObject(t.Name);
                    asset = gameObject;
                    break;
                case Type t when t.IsComponent():
                    var assetObject = new GameObject(t.Name,t);
                    asset = assetObject.GetComponent(t);
                    break;
            }

            return asset;
        }

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
                EditorUtility.DisplayProgressBar($"Progress [{i} of {count}] :", asset.ToString(), progress);
                action(asset);

            }
            
        }

        public static TAsset LoadOrCreate<TAsset>(this TAsset asset,string path,string assetName = "", bool refreshDatabase = false)
            where TAsset : ScriptableObject
        {
            assetName = string.IsNullOrEmpty(assetName) ? typeof(TAsset).Name : assetName;
            return asset ? asset : LoadOrCreate<TAsset>(path,assetName,refreshDatabase);
        }

        public static TAsset LoadOrCreate<TAsset>(string path,string assetName, bool refreshDatabase = false)
            where TAsset : ScriptableObject
        {
            assetName = string.IsNullOrEmpty(assetName) ? typeof(TAsset).Name : assetName;
            var asset = AssetEditorTools.GetAsset<TAsset>(path);
            if (asset) return asset;
            
            asset = ScriptableObject.CreateInstance<TAsset>();
            asset.SaveAsset(assetName, path,refreshDatabase);
            return asset;
        }
        
        public static bool OpenScript<T>(params string[] folders)
        {
            return OpenScript(typeof(T),folders);
        }

        public static bool OpenScript(this Type type,params string[] folders)
        {
            var asset = GetScriptAsset(type, folders);
            if (asset == null)
                return false;
            return AssetDatabase.OpenAsset(asset.GetInstanceID(), 0, 0);
        }

        public static MonoScript GetScriptAsset(this Type type, params string[] folders)
        {
            var typeName = type.Name;
            var filter   = $"t:script {typeName}";
            
            var assets = AssetDatabase.FindAssets(filter, folders);
            var assetGuid = assets.FirstOrDefault(
                x => string.Equals(typeName,
                    Path.GetFileNameWithoutExtension(x.AssetGuidToPath()),
                    StringComparison.OrdinalIgnoreCase));
            
            if (string.IsNullOrEmpty(assetGuid))
                return null;
            
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetGuid.AssetGuidToPath());
            return asset;
        }


        public static List<AttributeItemData<T,TAttr>> GetAssetsWithAttribute<T, TAttr>(string[] folders = null)
            where T : ScriptableObject
            where TAttr : Attribute
        {
            var result = new List<AttributeItemData<T,TAttr>>();
            
            var assets = GetAssets<T>(folders);

            foreach (var asset in assets) {
                var type = asset.GetType();
                var attribute = type.GetCustomAttribute<TAttr>();
                if (attribute != null) {
                    result.Add(new AttributeItemData<T, TAttr>() {
                        Attribute = attribute,
                        Value = asset
                    });
                }
            }

            return result;
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

        public static T GetAsset<T>(string folder) where T : Object
        {
            var asset = GetAssets<T>(new string[]{folder}).FirstOrDefault();
            return asset;
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

        public static EditorResource ToEditorResource(this Object asset, bool forceUpdate = false)
        {
            var editorResource = _editorResourceFactory(asset);
            if (forceUpdate)
                editorResource.Update();
            return editorResource;
        }
        
        public static List<T> GetAssets<T>(Type targetType,string folder) where T : Object
        {
            return GetAssets<T>(targetType, string.IsNullOrEmpty(folder) ? null : new[] {folder});
        }
        
        public static List<T> GetAssets<T>(Type targetType,string[] folders = null) where T : Object
        {
            var items = GetAssets(targetType, folders);
            return items.OfType<T>().ToList();
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
        
        public static IDisposable ShowActionProgress(IObservable<ProgressData> progressObservable, ILifeTime lifeTime)
        {

            var disposable = Disposable.Empty;
            var cancelation = lifeTime.AsCancellationSource();
            var isCanceled = EditorUtility.DisplayCancelableProgressBar(string.Empty, string.Empty, 0);
            if (isCanceled)
                return Disposable.Empty;
            
            disposable = progressObservable.
                Do(progress => {
                    isCanceled = EditorUtility.DisplayCancelableProgressBar(progress.Title, progress.Content, progress.Progress);
                    if (isCanceled) {
                        disposable.Cancel();
                        cancelation.Cancel();
                    }
                }).
                Finally(() => disposable.Cancel()).
                Finally(EditorUtility.ClearProgressBar).
                Subscribe().
                AddTo(lifeTime);
            
            return disposable;
            
        }

        public static bool ShowProgress(ProgressData progress)
        {
            if (progress.IsDone) {
                EditorUtility.ClearProgressBar();
                return true;
            }
            var isCanceled = EditorUtility.DisplayCancelableProgressBar(progress.Title, progress.Content, progress.Progress);
            if (isCanceled) {
                EditorUtility.ClearProgressBar();
            }

            return isCanceled;
        }
        
        public static void ShowProgress(IEnumerable<ProgressData> awaiter)
        {
            try {
                foreach (var progress in awaiter) {
                    var isCanceled = EditorUtility.
                        DisplayCancelableProgressBar(progress.Title, progress.Content, progress.Progress);
                    if (isCanceled)
                        break;
                }
            }
            finally{
                EditorUtility.ClearProgressBar();
            }
        }
        
        
        public static void ShowActionProgress(IEnumerator<ProgressData> awaiter)
        {
            var isShown = false;

            try {
                while (awaiter.MoveNext())
                {
                    isShown = true;
                    var progress   = awaiter.Current;
                    var isCanceled = EditorUtility.DisplayCancelableProgressBar(progress.Title, progress.Content, progress.Progress);
                    
                    if (isCanceled)
                        break;
                }
            }
            finally{
                if(isShown) EditorUtility.ClearProgressBar();
            }
        }
        
        public static string GetGUID(this Object asset)
        {
            if (!asset) return string.Empty;
            
            var path = AssetDatabase.GetAssetPath(asset);
            return string.IsNullOrEmpty(path) ? 
                string.Empty : 
                AssetDatabase.AssetPathToGUID(path);
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
