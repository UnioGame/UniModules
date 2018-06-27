using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ProfilerTools;
using ME;
using UniRx;

namespace UnityEngine.UI.Windows
{

    [System.Serializable]
    public struct TaskItem
    {

        public int id;
        public ResourceBase resource;
        public bool async;
        //public Graphic graphic;
        public string customResourcePath;
        public System.Action<object> onSuccess;
        public System.Action onFailed;

        public void RaiseSuccess(object obj)
        {

            this.onSuccess.Invoke(obj);

        }

        public void RaiseFailed()
        {

            this.onFailed.Invoke();

        }

        public void Dispose()
        {

            this.resource = null;
            //this.graphic = null;
            this.customResourcePath = null;
            this.onSuccess = null;
            this.onFailed = null;

        }

    };

    public class ResourcesProvider
    {

        private static Type ComponentType = typeof(Component);

        private static ResourcesProvider _instance;
        public static ResourcesProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ResourcesProvider();
                return _instance;
            }
        }

        [System.Serializable]
        public class Item
        {

#if UNITY_EDITOR
            public string name;
#endif

            public int id;
            public System.Type type;
            public System.Action<Item> onObjectLoaded;
            public System.Action<Item> onObjectFailed;

#if UNITY_EDITOR
            private int _referencesCount;
            public int referencesCount
            {

                set
                {

                    this._referencesCount = value;
                    this.UpdateName();

                }

                get
                {

                    return this._referencesCount;

                }

            }
#endif

            public object _loadedObject;
            public object loadedObject
            {

                set
                {

                    this._loadedObject = value;
#if UNITY_EDITOR
                    this.UpdateName();
#endif

                }

                get
                {

                    return this._loadedObject;

                }

            }

            public bool loaded;
            public bool loadingResult;

#if UNITY_EDITOR
            private void UpdateName()
            {

                if (this._loadedObject != null) this.name = string.Format("[{0}] {1} ({2})", this._referencesCount, (this._loadedObject != null) ? (this._loadedObject is Object ? (this._loadedObject as Object).name : this._loadedObject.ToString()) : "Null", (this.type == null ? "Null" : this.type.ToString()));

            }
#endif

            public void Dispose(ResourceBase resource)
            {

                resource.Unload();
                this.loadedObject = null;
                this.onObjectLoaded = null;
                this.id = 0;
                this.type = null;
                this.loaded = false;

#if UNITY_EDITOR
                this.referencesCount = 0;
#endif

            }

        };

        [System.Serializable]
        public class ObjectEntity
        {

            public string name;
            public int instanceId;

        };

        public List<ObjectEntity> registered = new List<ObjectEntity>();

        public bool forceAsyncOff = true;
        public float resourceAsyncLoadFadeTime = 0.5f;
        public List<Item> loaded = new List<Item>();

        [System.Serializable]
        public struct ResourceEntity
        {

            public Object resource;

        };

        [System.Serializable]
        public class PreloadedResourceEntity
        {

            public long key;
            public Object resource;

        };

        public List<PreloadedResourceEntity> preloadedResources = new List<PreloadedResourceEntity>();

        public Dictionary<long, ResourceEntity> resources = new Dictionary<long, ResourceEntity>();

        public static T Load<T>(string path) where T : Object
        {

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {

                return Resources.Load<T>(path);

            }
#endif

            return ResourcesProvider.Instance.Load_INTERNAL(path, typeof(T)) as T;

        }

        public static Object Load(string path, System.Type type)
        {

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {

                return Resources.Load(path, type);

            }
#endif

            return ResourcesProvider.Instance.Load_INTERNAL(path, type);

        }

        public static IEnumerator LoadBundleResourceAsync<T>(string bundleName, string assetName, System.Action<T> resultCallback) where T : Object
        {

            var operation = AssetBundleManager.Instance.LoadAsset(bundleName);
            yield return operation.Execute();
            yield return operation.LoadAssetByNameAsync<T>(assetName, resultCallback);

        }

        public static T LoadBundleResource<T>(string bundleName, string assetName) where T : Object
        {

            if (string.IsNullOrEmpty(bundleName) == true || string.IsNullOrEmpty(assetName) == true)
                return null;

            var asset = LoadBundleResourceSync<T>(bundleName, assetName);

            return asset;

        }

        public static T LoadBundleResource<T>() where T : Object
        {

            var resource = typeof(T).Name;
            var asset = LoadBundleResourceSync<T>(resource.ToLowerInvariant(), resource);
            return asset;

        }

        public static IEnumerator LoadBundleResourcesAsync<T>(string bundleName, System.Action<List<T>> callback) where T : Object
        {

            if (callback == null) yield break;

            List<T> assets = null;
            var operation = AssetBundleManager.Instance.LoadAsset(bundleName);
            yield return operation.Execute();
            yield return operation.LoadAssetsAsync<T>(x => assets = x);
            callback(assets);

        }

        public static List<T> LoadBundleResources<T>(string bundleName) where T : Object
        {

            var operation = AssetBundleManager.Instance.LoadAsset(bundleName);
            operation.Load();
            var assets = operation.LoadAssets<T>();
            return assets;

        }

        public static T LoadBundleResourceSync<T>(string bundleName, string assetName) where T : Object
        {

            if (string.IsNullOrEmpty(bundleName) == true || string.IsNullOrEmpty(assetName) == true)
                return null;

            var operation = AssetBundleManager.Instance.LoadAsset(bundleName);
            operation.Load();
            var result = operation.LoadAsset<T>(assetName);
            return result;

        }

        private void LoadResourceFromBundle<T>(TaskItem task) where T : Object
        {
            var resource = task.resource;
            var assetName = resource.resourcesPath;
            var provider = AssetBundleManager.Instance.LoadAsset(resource.assetBundleName);

            provider.Load();

            Object asset = provider.LoadAsset<T>(assetName);

            if (asset == null)
            {

                task.RaiseFailed();

            }
            else
            {

                task.RaiseSuccess(asset);

            }

        }


        private IEnumerator LoadResourceFromBundleAsync<T>(TaskItem task) where T : Object
        {
            var resource = task.resource;
            var assetName = resource.resourcesPath;
            var provider = AssetBundleManager.Instance.LoadAsset(resource.assetBundleName);
            yield return provider.Execute();
            Object asset = null;
            yield return provider.LoadAssetByNameAsync<T>(assetName, x =>
            {

                asset = x;

            });
            if (asset == null)
            {

                task.RaiseFailed();

            }
            else
            {

                task.RaiseSuccess(asset);

            }

        }

        private Object Load_INTERNAL(string path, System.Type type)
        {

            var pathHash = ResourceBase.GetJavaHash(path);
            var typeHash = ResourceBase.GetJavaHash(type.FullName);
            var resourceKey = ResourceBase.GetKey(pathHash, typeHash);

            Object output = null;
            ResourceEntity entity;

            if (this.resources.TryGetValue(resourceKey, out entity) == true)
            {
                output = entity.resource;
            }

            if (output == null)
            {

                var key = ResourceBase.GetKey(pathHash, 0);
                if (this.resources.TryGetValue(key, out entity) == true)
                {

                    if (entity.resource is GameObject && type != typeof(GameObject))
                    {

                        output = (entity.resource as GameObject).GetComponent(type);

                    }
                    else
                    {

                        output = entity.resource;

                    }

                }

            }

            if (output == null)
            {

                var id = GameProfiler.BeginWatch("Load_INTERNAL Resources.Load " + path);

                output = Resources.Load(path, type);
                this.resources[resourceKey] = new ResourceEntity() { resource = output };

                GameProfiler.StopWatch(id);

            }

            return output;

        }


        public static object GetResourceObjectById(ResourceBase resource)
        {

            var item = ResourcesProvider.GetItem(resource);
            if (item == null) return null;

            return item.loadedObject;

        }

        public static bool IsResourceObjectLoaded(ResourceBase resource)
        {

            var item = ResourcesProvider.GetItem(resource);
            if (item == null) return false;

            return item.loaded;

        }

        private static bool RemoveIfRefsZero_INTERNAL(Item item, ResourceBase resource)
        {

            if (item == null) return false;

            item.Dispose(resource);
            ResourcesProvider.Instance.loaded.RemoveAll(x =>
            {

                if (x.id == item.id && x.type == item.type)
                {

                    return true;

                }

                return false;

            });

            return true;

        }

        public static Item GetItem(ResourceBase resource)
        {

            return ResourcesProvider.Instance.loaded.FirstOrDefault(x => x.id == resource.GetId());

        }

        public static float GetAsyncLoadFadeTime()
        {

            return ResourcesProvider.Instance.resourceAsyncLoadFadeTime;

        }

        private void OnResourceCallBack<T>(T data, Item item) where T : Object
        {

            if (data == null)
            {

                item.loadingResult = false;
                if (item.onObjectFailed != null)
                {

                    item.onObjectFailed.Invoke(item);

                }

                item.onObjectLoaded = null;
                item.onObjectFailed = null;
                return;

            }

            item.loadingResult = true;
            item.loadedObject = data;
            item.loaded = true;

            if (item.onObjectLoaded != null) item.onObjectLoaded.Invoke(item);
            item.onObjectLoaded = null;
            item.onObjectFailed = null;
        }

        private void OnItemCallback_INTERNAL<T>(Item item, ResourceBase resource, System.Action<T> callbackLoaded, System.Action callbackFailed) /*where T : Object*/
        {

            item.onObjectLoaded += (itemInner) =>
            {

                if (ResourcesProvider.RemoveIfRefsZero_INTERNAL(itemInner, resource) == true) return;

                if (callbackLoaded != null) callbackLoaded.Invoke((T)itemInner.loadedObject);

            };

            item.onObjectFailed += (itemInner) =>
            {

                if (callbackFailed != null) callbackFailed.Invoke();

            };

        }

        public static void UnloadBundleResource(string bundleName, bool forceUnload = false)
        {
            AssetBundleManager.Instance.UnloadAssetBundle(bundleName, forceUnload);
        }


        private List<TaskItem> tasks = new List<TaskItem>();

        public static void LoadResource<T>(ResourceBase resource, System.Action<T> callback, bool async) where T : Object
        {

            Observable.FromCoroutine(x => Instance.LoadResource_INTERNAL<T>(resource, null, callback, async));

        }

        public static IEnumerator LoadResource<T>(ResourceBase resource,
            string customResourcePath, System.Action<T> callback, bool async) where T : Object
        {

            return Instance.LoadResource_INTERNAL<T>(resource, customResourcePath, callback, async);

        }

        private IEnumerator LoadResource_INTERNAL<T>(ResourceBase resource,
            string customResourcePath, System.Action<T> callback, bool async) where T : Object
        {

            if (resource.HasDirectRef() == true)
            {

                callback.Invoke(resource.GetDirectRef<T>());
                yield break;

            }

            if (this.forceAsyncOff == true)
            {

                // Disabling async resource loading on Android, because for some reason Resources.LoadAsync() is terribly slow
                // as of Unity 5.6.2p2 (takes ~2 minutes to load some resources).
                async = false;

            }

            var task = new TaskItem();
            task.id = resource.GetId();
            task.async = async;
            task.resource = resource;
            task.customResourcePath = customResourcePath;
            task.onSuccess = (obj) =>
            {

                T resultObj = default(T);
                var gameObject = obj as GameObject;

                if (gameObject && ComponentType.IsAssignableFrom(typeof(T)))
                {

                    resultObj = gameObject.GetComponent<T>();

                }
                else
                {

                    resultObj = (T)obj;

                }

                task.resource.tempObject = resultObj;
                callback.Invoke(resultObj);

            };
            task.onFailed = () =>
            {

                callback.Invoke(default(T));

            };

#if UNITY_EDITOR
            this.tasks.Add(task);
#endif

            var coroutine = this.StartTask<T>(task);
            while (coroutine.MoveNext() == true)
            {

                yield return 0;

            }

            task.Dispose();

#if UNITY_EDITOR
            this.tasks.RemoveAll(x => x.id == task.id);
#endif

        }

        private IEnumerator<byte> StartTask<T>(TaskItem task) where T : Object
        {

            #region Load Resource

            var resource = task.resource;

            if (resource.loadableWeb == true)
            {

                var enumerator = LoadFromWeb<T>(task);
                while (enumerator.MoveNext())
                {
                    yield return 0;
                }

            }
            else if (resource.loadableResource == true || (string.IsNullOrEmpty(task.customResourcePath) == false &&
                                                           resource.loadableAssetBundle == false))
            {

                var enumerator = LoadFromResources<T>(task);
                while (enumerator.MoveNext())
                {
                    yield return 0;
                }

            }
            else if (resource.loadableStream == true)
            {

                var enumerator = LoadFromStream<T>(task);
                while (enumerator.MoveNext())
                {
                    yield return 0;
                }

            }
            else if (resource.loadableAssetBundle == true)
            {
                var id = GameProfiler.BeginWatch(string.Format("Load Asset [{0}] From Bundle [{1}]",
                    resource.assetPath, resource.assetBundleName));
                GameProfiler.BeginSample("loadableAssetBundle");

                if (resource.loadResourceAsync == true)
                {

                    System.IDisposable operation = null;
                    operation = Observable.FromCoroutine(() => LoadResourceFromBundleAsync<T>(task))
                        .DoOnCompleted(() => operation = null).Subscribe();
                    while (operation != null)
                        yield return 0;
                }
                else
                {
                    LoadResourceFromBundle<T>(task);
                }

                GameProfiler.EndSample();
                GameProfiler.StopWatch(id);
            }

            #endregion


        }

        private IEnumerator<byte> LoadFromWeb<T>(TaskItem task)
        {
            if (task.resource.resourcesPath.Contains("://") == false)
            {

                task.resource.resourcesPath = string.Format("file://{0}", task.resource.resourcesPath);

            }

            if (task.resource.webPath.Contains("file://") == false && Application.internetReachability == NetworkReachability.NotReachable)
            {

                task.RaiseFailed();
                yield break;

            }

            WWW www = null;
            if (task.resource.cacheVersion > 0)
            {

                www = WWW.LoadFromCacheOrDownload(task.resource.resourcesPath, task.resource.cacheVersion);

            }
            else
            {

                www = new WWW(task.resource.resourcesPath);

            }

            while (www.isDone == false) yield return 0;

            if (string.IsNullOrEmpty(www.error) == true)
            {

                var type = typeof(T);
                if (type == typeof(Texture) ||
                    type == typeof(Texture2D))
                {

                    task.RaiseSuccess(task.resource.readableTexture == true ? www.texture : www.textureNonReadable);

                }
                else
                {

                    task.RaiseSuccess(www.bytes);

                }

            }
            else
            {

                GameLog.LogError(string.Format("Task WebRequest [{0}] error: {1}", www.url, www.error));
                task.RaiseFailed();

            }

            www.Dispose();
            www = null;
        }

        private IEnumerator<byte> LoadFromResources<T>(TaskItem task)
        {
            var isBytesOutput = (typeof(T) == typeof(byte[]));

            Object data = null;
            var resourcePath = task.customResourcePath ?? task.resource.resourcesPath;
            if (task.async == true)
            {

                var asyncTask = Resources.LoadAsync(resourcePath, isBytesOutput == true ? typeof(TextAsset) : typeof(T));
                while (asyncTask.isDone == false)
                {

                    yield return 0;

                }

                data = asyncTask.asset;
                asyncTask = null;

            }

            if (task.resource.multiObjects == true && task.resource.objectIndex >= 0)
            {

                task.RaiseSuccess(Resources.LoadAll(resourcePath)[task.resource.objectIndex]);

            }
            else
            {

                if (isBytesOutput == true)
                {

                    if (data == null) data = UnityEngine.UI.Windows.ResourcesProvider.Load<TextAsset>(resourcePath);
                    task.RaiseSuccess(((data as TextAsset).bytes));

                }
                else
                {

                    if (data == null) data = UnityEngine.UI.Windows.ResourcesProvider.Load(resourcePath, typeof(T));
                    task.RaiseSuccess(data);

                }

            }
        }

        private IEnumerator<byte> LoadFromStream<T>(TaskItem task)
        {

            var isBytesOutput = (typeof(T) == typeof(byte[]));

            var path = task.resource.GetStreamPath(withFile: true);
            var www = task.resource.cacheVersion > 0 ?
                WWW.LoadFromCacheOrDownload(path, task.resource.cacheVersion) :
                new WWW(path);

            while (www.isDone == false) yield return 0;

            if (string.IsNullOrEmpty(www.error) == true)
            {

                var type = typeof(T);
                if (type == typeof(Texture) || type == typeof(Texture2D))
                {

                    var texture = task.resource.readableTexture == true ? www.texture : www.textureNonReadable;
                    task.RaiseSuccess(texture);

                }
                else
                {

                    var data = www.bytes;
                    if (isBytesOutput == true)
                    {

                        task.RaiseSuccess(data);

                    }
                    else
                    {

                        task.RaiseSuccess(null);

                    }

                }

            }
            else
            {

                GameLog.Log("NOT LOADED: " + task.resource.GetStreamPath(withFile: true) + " :: " + www.error);
                task.RaiseFailed();

            }

            www.Dispose();
            www = null;
        }

#if UNITY_EDITOR


        public Object[] preloadedResourcesEditor;
        public Object[] prevResourcesEditor;

        public static readonly Dictionary<UnityEditor.BuildTarget, string> ALL_TARGETS = new Dictionary<UnityEditor.BuildTarget, string>() {

            { UnityEditor.BuildTarget.Android, "Android" },
            { UnityEditor.BuildTarget.iOS, "iOS" },

            { UnityEditor.BuildTarget.PS4, "PS4" },
            { UnityEditor.BuildTarget.XboxOne, "XboxOne" },
            { UnityEditor.BuildTarget.tvOS, "tvOS" },
            { UnityEditor.BuildTarget.Switch, "Switch" },

            { UnityEditor.BuildTarget.StandaloneLinuxUniversal, "Linux" },
            { UnityEditor.BuildTarget.StandaloneOSXUniversal, "Mac" },
            { UnityEditor.BuildTarget.StandaloneWindows, "Windows" },

        };

#endif

    }

}