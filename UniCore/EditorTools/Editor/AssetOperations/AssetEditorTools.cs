using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UniModule.UnityTools.ReflectionUtils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.EditorTools
{
    using UniGreenModules.UniCore.EditorTools.AssetOperations;

    public class AssetEditorTools
    {

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
                return "prefab";
            if (asset is ScriptableObject)
                return "asset";
            return string.Empty;
        }
        
        #region Asset Creation/Saving
        
        public static TAsset SaveAsset<TAsset>(TAsset asset,string name, string folder)
            where  TAsset : Object
        {
            if (folder.IndexOf('\\', folder.Length - 1)>=0)
            {
                folder = folder.Remove(folder.Length - 1);
            }

                        
            var skinTypePath = folder + "\\" + name + "." + GetAssetExtension(asset);
            var itemPath     = AssetDatabase.GenerateUniqueAssetPath(skinTypePath);  
            
            
            var gameObjectAsset = asset as GameObject;
            if (gameObjectAsset!= null)
            {
                gameObjectAsset.name = name;
                return PrefabUtility.
                    SaveAsPrefabAssetAndConnect(gameObjectAsset,itemPath,InteractionMode.AutomatedAction) as TAsset;
            }
            
            AssetDatabase.CreateAsset(asset, itemPath);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<TAsset>(itemPath);
        }

        public static bool SaveAssetAsNested(Object child, Object root, string name = null)
        {
            var assetPath = AssetDatabase.GetAssetPath(root);
            if (string.IsNullOrEmpty(assetPath))
                return false;
            
            var childName = name == null ? child.name : name;
            childName = string.IsNullOrEmpty(childName) ? child.GetType().Name : childName;
            child.name = childName;
            
            AssetDatabase.AddObjectToAsset(child,root);

            EditorUtility.SetDirty(root);
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();
            
            return true;
        }
        
        public static Object SaveAssetAsNested(Object root,Type assetType, string name = null)
        {
            var asset  = ScriptableObject.CreateInstance(assetType);
            var result = SaveAssetAsNested(asset,root, name);
            if (result) return asset;
            return null;
        }

        public static TTarget SaveAssetAsNested<TTarget>(Object root, string name = null)
            where TTarget : ScriptableObject
        {
            var asset  = ScriptableObject.CreateInstance<TTarget>();
            var result = SaveAssetAsNested(asset,root, name);
            if (result) return asset;
            return null;
        }

        public static TAsset CreateAsset<TAsset>(string name, string folder)
            where  TAsset : ScriptableObject
        {
            if (folder.IndexOf('\\', folder.Length - 1)>=0)
            {
                folder = folder.Remove(folder.Length - 1);
            }

            var asset        = ScriptableObject.CreateInstance<TAsset>();
            var skinTypePath = folder + "\\" + name + ".asset";
            var itemPath     = AssetDatabase.GenerateUniqueAssetPath(skinTypePath);
            AssetDatabase.CreateAsset(asset, itemPath);

            AssetDatabase.SaveAssets();

            return asset;

        }

        [MenuItem("Assets/Set Selected Dirty")]
        public static void SetSelectedAsDirty()
        {
            var selectedFolders = GetActiveAssets();
            if (selectedFolders.Count > 0)
            {
                var folderAssets = GetAssets<Object>(selectedFolders.Select(x => x.assetPath).ToArray());
                foreach (var folderAsset in folderAssets)
                {
                    EditorUtility.SetDirty(folderAsset);
                }
            }
            var selectedItems = Selection.objects.ToList();
            foreach (var asset in selectedItems)
            {
                SetDirty(asset);
            }
            if (Selection.gameObjects.Length == 0) return;
            foreach (var asset in Selection.gameObjects)
            {
                SetDirty(asset);
            }
        }

        [MenuItem("Tools/Remove ALL modifications")]
        public static void SetAllDirty()
        {

            var assets = new List<Object>();
            assets.AddRange(GetAssets<GameObject>().OfType<Object>());
            assets.AddRange(GetAssets<ScriptableObject>().OfType<Object>());

            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                var progress = i / ((float)assets.Count);
                var canceled = EditorUtility.DisplayCancelableProgressBar(
                    string.Format("Remove Modifications [{0} of {1}] :", i, assets.Count), asset.name, progress);
                if (canceled) break;
                RemoveModifications(assets[i]);
            }
            EditorUtility.ClearProgressBar();
        }

        public static void SetDirty(Object asset)
        {
            RemoveModifications(asset);
            EditorUtility.SetDirty(asset);
        }

        public static void SetDirty<T>() where T : Object
        {

            var assets = AssetEditorTools.GetAssets<T>();
            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                if (asset == null) continue;
                EditorUtility.SetDirty(asset);
            }

        }

        public static void RemoveModifications(Object asset)
        {

            if (string.IsNullOrEmpty(clientDataPath))
            {
                clientDataPath    = Application.dataPath.Replace("Assets", "");
                modificationRegex = new Regex(ModificationTemplate);
            }

            var assetPath = AssetDatabase.GetAssetPath(asset);

            var ext = Path.GetExtension(assetPath);
            if (_modificationsIgnoreList.Contains(ext)) return;

            var dataPath = string.Format("{0}/{1}", clientDataPath, assetPath);

            if (File.Exists(dataPath))
            {
                var text   = File.ReadAllText(dataPath);
                var result = modificationRegex.Replace(text, "");
                File.WriteAllText(dataPath, result);
            }
        }

        #endregion
        
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

        public static void ApplyBundleToAsset<T>() where T : Object
        {
            ApplyBundleToAsset<T>(typeof(T).Name.ToLowerInvariant());
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

        public static string GetUniqueAssetName(string path)
        {
            return AssetDatabase.GenerateUniqueAssetPath(path);
        }

        #region Assets Bundle Operations
        
        public static string GetValidBundleTag(string bundleTag)
        {

            if (string.IsNullOrEmpty(bundleTag)) return string.Empty;
            var parentName = bundleTag.Replace(".", "-");
            parentName = parentName.Replace(" ", "-");
            parentName = parentName.Replace("?", "");
            parentName = parentName.Replace("_", "-");
            return parentName;

        }

        public static void ApplyBundleToAssetWithTemplate(Type targetType, string bundleTag, string variant = null)
        {

            var assets       = GetAssetImporters(targetType);
            var useTemplates = IsContainsDuplicatedAssets(assets);
            ApplyBundleTagToAsset(assets, bundleTag, variant, useTemplates);
        }

        public static void ApplyBundleToAsset(Type targetType, string bundleTag, string variant = null, bool useTemplate = false)
        {
            var assets       = GetAssetImporters(targetType);
            var isDuplicated = IsContainsDuplicatedAssets(assets);
            if (isDuplicated)
            {
                Debug.LogWarningFormat("FOUND DUPLICATED items in {0} BUNDLE Tag", bundleTag);
            }

            ApplyBundleTagToAsset(assets, bundleTag, variant);
        }

        public static void ApplyBundleTagToAsset(List<AssetImporter> assets, string bundleTag, string variant = null,
            bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            for (var i = 0; i < assets.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Apply Bundle Tag", string.Format("Process...[{0}:{1}]", i, assets.Count), i / (float)assets.Count);
                ApplyBundleTag(assets[i], bundleTag, variant, useTemplate, depth);
            }
            EditorUtility.ClearProgressBar();
        }

        public static string GetBundleName(string path)
        {
            if (string.IsNullOrEmpty(path) == true) return string.Empty;
            var abName = AssetDatabase.GetImplicitAssetBundleName(path);
            return string.IsNullOrEmpty(abName) ? string.Empty : abName;
        }

        public static void ApplyBundleTag(Object[] assets, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {

            var count = assets.Length;
            for (var i = 0; i < count; i++)
            {

                var asset = assets[i];
                if (asset == null)
                    continue;
                EditorUtility.DisplayProgressBar("Apply Screen Bundle Tag",
                    string.Format("Process...[{0}:{1}]", i, count), i / (float)count);

                var path     = AssetDatabase.GetAssetPath(asset);
                var importer = AssetImporter.GetAtPath(path);
                AssetEditorTools.ApplyBundleTag(importer, bundleTag, variant, useTemplate, depth, startDepth);

            }

            EditorUtility.ClearProgressBar();
        }

        public static bool IsInBundle(Object asset)
        {
            if (asset == null) return false;
            var path = AssetDatabase.GetAssetPath(asset);
            return IsInBundle(path);
        }

        public static bool IsInBundle(string path)
        {
            if (string.IsNullOrEmpty(path) == true) return false;
            var abName = AssetDatabase.GetImplicitAssetBundleName(path);
            return string.IsNullOrEmpty(abName) == false;
        }

        public static string ApplyBundleTag(string assetPath, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            if (string.IsNullOrEmpty(assetPath)) return string.Empty;
            var importer = AssetImporter.GetAtPath(assetPath);
            return ApplyBundleTag(importer, bundleTag, variant, useTemplate, depth, startDepth);
        }

        public static string ApplyBundleTag(AssetImporter importer, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {

            if (importer == null) return string.Empty;

            var assetBundleName = bundleTag;
            if (useTemplate)
            {

                var tag = (string.IsNullOrEmpty(bundleTag) ? string.Empty : bundleTag + "-");
                assetBundleName = tag + GetFoldersTemplateName(importer.assetPath, depth, false, startDepth);
            }

            assetBundleName = GetValidBundleTag(assetBundleName);
            var assetBundleVariant = GetValidBundleTag(variant);

            if (importer.assetBundleName == assetBundleName)
                return importer.assetBundleName;

            importer.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);

            return assetBundleName;
        }

        public static string GetBundleTag(string bundleTag,string assetPath = null, bool useTemplate = false, int depth = 4, int startDepth = 0) {
            var assetBundleName = bundleTag;
            if (string.IsNullOrEmpty(assetPath) ==false && useTemplate)
            {
                var tag = (string.IsNullOrEmpty(bundleTag) ? string.Empty : bundleTag + "-");
                assetBundleName = tag + GetFoldersTemplateName(assetPath, depth, false, startDepth);
            }

            assetBundleName = GetValidBundleTag(assetBundleName);
            return assetBundleName;
        }

        public static void ApplyBundleToAsset(Type targetType, bool useTemplate = false)
        {
            if (useTemplate)
            {
                ApplyBundleToAssetWithTemplate(targetType, targetType.Name);
                return;
            }
            ApplyBundleToAsset(targetType, targetType.Name);
        }

        public static void ApplyBundleToAsset<T>(string bundleTag, string variant = null) where T : Object
        {
            ApplyBundleToAsset(typeof(T), bundleTag, variant);
        }
        
        #endregion
        
        #region Asset Importers
        
        public static List<AssetImporter> GetActiveAssets(bool foldersOnly = true)
        {
            var assets  = new List<AssetImporter>();
            var targets = Selection.objects;
            for (int i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                var path   = AssetDatabase.GetAssetPath(target);
                var asset = foldersOnly ? GetDirectoryImporter(path) :
                    AssetImporter.GetAtPath(path);
                if (asset)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static AssetImporter GetDirectoryImporter(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var directory = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
            return AssetImporter.GetAtPath(directory);
        }
        
        public static List<AssetImporter> GetAssetImporters(string item, string[] folders = null, bool foldersOnly = false)
        {
            var assetImporters = new List<AssetImporter>();
            var filterText = item;
            var ids = AssetDatabase.FindAssets(filterText, folders);
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var path = AssetDatabase.GUIDToAssetPath(id);
                if (foldersOnly && Directory.Exists(path) == false)
                    continue;
                var importer = AssetImporter.GetAtPath(path);
                assetImporters.Add(importer);
            }

            return assetImporters;
        }

        public static List<AssetImporter> GetAssetImporters<T>(string[] folders = null, bool foldersOnly = false) where T : Object
        {
            return GetAssetImporters($"t:{typeof(T).Name}", folders, foldersOnly);
        }

        public static List<AssetImporter> GetAssetImporters(Type targetType, string[] folders = null, bool foldersOnly = false)
        {
            return GetAssetImporters(string.Format("t:{0}", targetType.Name), folders, foldersOnly);
        }

        public static List<string> GetAssetsPaths<T>(string[] folders = null) where T : Object
        {
            var assetsPaths = new List<string>();
            var filter = $"t:{typeof(T).Name}";
            var ids = AssetDatabase.FindAssets(filter, folders);
            
            for (var i = 0; i < ids.Length; i++) {
                var id = ids[i];
                var path = AssetDatabase.GUIDToAssetPath(id);
                assetsPaths.Add(path);   
            }

            return assetsPaths;
        }
        
        #endregion 
        
        #region asset loading
        
        public static List<Object> GetAssets(Type assetType, string[] folders = null)
        {
            var filterText = $"t:{nameof(assetType)}";
            var assets = GetAssets<Object>(filterText, folders);
            return assets;
        }

        public static List<T> GetAssets<T>(string filter, string[] folders = null) where T : Object
        {
            var assets = new List<T>();
            ShowActionProgress(GetAssets(assets, filter, folders));
            return assets;
        }

        public static List<T> GetAssetsByPaths<T>(List<string> paths) where T : Object
        {
            var assets = new List<T>();
            for (var i = 0; i < paths.Count; i++) {
                var path = paths[i];
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if(!asset) continue;
                assets.Add(asset);
            }

            return assets;
        }
        
        public static IEnumerator<ProgressData> GetAssets<T>(List<T> resultContainer, string filter, string[] folders = null) where T : Object
        {

            var progress = new ProgressData()
            {
                Content = "loading...",
                Title = "GetAsset of type " + typeof(T).Name,
            };

            yield return progress;

            var type = typeof(T);
            var ids = AssetDatabase.FindAssets(filter, folders);
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (string.IsNullOrEmpty(assetPath))
                {
                    Debug.LogErrorFormat("Asset importer {0} with NULL path detected", id);
                    continue;
                }
                T asset = null;

                asset = AssetDatabase.LoadAssetAtPath(assetPath, type) as T;

                if (asset) resultContainer.Add(asset);

                progress.Progress = (float)i / ids.Length;

                yield return progress;
            }
        }
        
        #endregion
        
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

        public static bool IsComponent(Type targetType)
        {
            return _componentType.IsAssignableFrom(targetType);
        }

        #region private methods
        
        private static bool IsContainsDuplicatedAssets(List<AssetImporter> assets)
        {
            var assetsNames = assets.Select(x => Path.GetFileNameWithoutExtension(x.assetPath)).ToList();
            var groups      = assetsNames.GroupBy(x => x).ToList();
            return groups.Count != assets.Count;
        }

        #endregion
    }

}
