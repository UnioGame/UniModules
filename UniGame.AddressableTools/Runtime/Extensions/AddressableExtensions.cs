namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Runtime.Extension;
    using global::UniCore.Runtime.ProfilerTools;
    using SerializableContext.Runtime.Addressables;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

    public static class AddressableExtensions
    {
        public static async UniTask<SceneInstance> LoadSceneTaskAsync(
            this AssetReference sceneReference,
            ILifeTime lifeTime,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            int priority = 100)
        {
            if (sceneReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {sceneReference}");
                return default;
            }

            var dependencies = Addressables.DownloadDependenciesAsync(sceneReference.RuntimeKey);
            dependencies.AddTo(lifeTime);
            if(dependencies.Task != null)
                await dependencies.Task;
            
            var sceneHandle = sceneReference.LoadSceneAsync(loadSceneMode, activateOnLoad, priority);
            //add to resource unloading
            sceneHandle.AddTo(lifeTime);

            await sceneHandle.Task;
            
            return sceneHandle.IsValid() ? sceneHandle.Result : default;
        }

        public static void UnloadReference(this AssetReference reference)
        {
#if UNITY_EDITOR
            var targetAsset = reference.editorAsset;
            GameLog.Log($"UNLOAD AssetReference {targetAsset?.name} : {reference.AssetGUID}");    
#endif
            // if(reference.Asset is IDisposable disposable)
            //     disposable.Dispose();
            //
            reference.ReleaseAsset();
        }

        public static async UniTask<List<TResult>> LoadScriptableAssetsTaskAsync<TResult>(
            this IEnumerable<AssetReference> assetReference, 
            ILifeTime lifeTime)
            where TResult : class
        {
            var container = new List<TResult>();
            await assetReference.LoadAssetsTaskAsync<ScriptableObject, TResult, AssetReference>(container, lifeTime);
            return container;
        }
        
        public static async UniTask<IEnumerable<TSource>> LoadAssetsTaskAsync<TSource, TAsset>(
            this IEnumerable<TAsset> assetReference, 
            List<TSource> resultContainer, ILifeTime lifeTime)
            where TAsset : AssetReference
            where TSource : Object
        {
            return await assetReference.LoadAssetsTaskAsync<TSource, TSource, TAsset>(resultContainer, lifeTime);
        }
        
        public static async UniTask<IEnumerable<TResult>> LoadAssetsTaskAsync<TSource,TResult,TAsset>(
            this IEnumerable<TAsset> assetReference, 
            IList<TResult> resultContainer, ILifeTime lifeTime)
            where TResult : class
            where TAsset : AssetReference
            where TSource : Object
        {
            var taskList = ClassPool.Spawn<List<UniTask<TSource>>>();

            foreach (var asset in assetReference) {
                var assetTask = asset.LoadAssetTaskAsync<TSource>(lifeTime);
                taskList.Add(assetTask);
            }

            var result = await UniTask.WhenAll(taskList);
            for (var j = 0; j < result.Length; j++) {
                if(result[j] is TResult item) resultContainer.Add(item);
            }
            
            taskList.Despawn();

            return resultContainer;
        }
        
        public static async UniTask<IReadOnlyList<TSource>> LoadAssetsTaskAsync<TSource, TAsset>(
            this IReadOnlyList<TAsset> assetReference, 
            List<TSource> resultContainer, ILifeTime lifeTime)
            where TAsset : AssetReference
            where TSource : Object
        {
            return await assetReference.LoadAssetsTaskAsync<TSource, TSource, TAsset>(resultContainer,lifeTime);
        }

        public static async UniTask<IReadOnlyList<TResult>> LoadAssetsTaskAsync<TSource,TResult,TAsset>(
            this IReadOnlyList<TAsset> assetReference, 
            List<TResult> resultContainer,ILifeTime lifeTime)
            where TResult : class
            where TAsset : AssetReference
            where TSource : Object
        {
            var taskList = ClassPool.Spawn<List<UniTask<TSource>>>();
            
            for (var i = 0; i < assetReference.Count; i++) {
                var asset = assetReference[i];
                var assetTask = asset.LoadAssetTaskAsync<TSource>(lifeTime);
                taskList.Add(assetTask);
            }
            
            var result = await UniTask.WhenAll(taskList);
            for (var j = 0; j < result.Length; j++) {
                if(result[j] is TResult item) resultContainer.Add(item);
            }
            
            taskList.Despawn();

            return resultContainer;
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(this AssetReference assetReference,ILifeTime lifeTime)
            where T : Object
        {
            if (assetReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return null;
            }
            
            var dependencies = Addressables.DownloadDependenciesAsync(assetReference.RuntimeKey);
            dependencies.AddTo(lifeTime);
            if(dependencies.Task != null)
                await dependencies.Task;

            var isComponent = typeof(T).IsComponent();
            var asset = isComponent ? 
                await LoadAssetAsync<GameObject>(assetReference,lifeTime) :
                await LoadAssetAsync<T>(assetReference,lifeTime);

            var result = default(T);
            if (asset == null)
                return result;
            
            result = asset is GameObject gameObjectAsset && isComponent ? 
                gameObjectAsset.GetComponent<T>():
                asset as T;

            return result;
            
        }

        public static async UniTask<(TAsset,TResult)> LoadAssetTaskAsync<TAsset,TResult>(
            this AssetReference assetReference,ILifeTime lifeTime)
            where TAsset : Object
            where TResult : class
        {
            var result = await assetReference.LoadAssetTaskAsync<TAsset>(lifeTime);
            return (result,result as TResult);
        }

        
        public static async UniTask<T> LoadAssetTaskAsync<T>(
            this AssetReferenceGameObject assetReference,
            ILifeTime lifeTime)
            where T : class
        {
            var result = await LoadAssetTaskAsync<GameObject>(assetReference as AssetReference,lifeTime);
            if (result is T tResult) return tResult; 
            return result != null ? result.GetComponent<T>() : null;
        }

        public static async UniTask<T> LoadAssetTaskAsync<T>(
            this AssetReferenceScriptableObject<T> assetReference, 
            ILifeTime lifeTime)
            where T : class
        {
            var result = await LoadAssetTaskAsync<ScriptableObject>(assetReference as AssetReference,lifeTime);
            return result as T;
        }
        
        public static async UniTask<TApi> LoadAssetTaskAsync<T,TApi>(
            this AssetReferenceScriptableObject<T,TApi> assetReference, 
            ILifeTime lifeTime)
            where T : ScriptableObject 
            where TApi : class
        {
            var result = await LoadAssetTaskAsync<ScriptableObject>(assetReference,lifeTime);
            return result as TApi;
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(
            this AssetReferenceScriptableObject assetReference, 
            ILifeTime lifeTime)
            where T : class
        {
            var result = await LoadAssetTaskAsync<ScriptableObject>(assetReference as AssetReference,lifeTime);
            return result as T;
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(this AssetReferenceT<T> assetReference, ILifeTime lifeTime)
            where T : Object
        {
            return await LoadAssetTaskAsync<T>(assetReference as AssetReference,lifeTime);
        }
        
        public static async UniTask<T> LoadGameObjectAssetTaskAsync<T>(this AssetReferenceT<T> assetReference, ILifeTime lifeTime)
            where T : Component
        {
            var result = await LoadAssetTaskAsync<GameObject>(assetReference ,lifeTime);
            return result ? 
                result.GetComponent<T>() : 
                null;
        }
        
        public static async UniTask<T> LoadGameObjectAssetTaskAsync<T>(this AssetReference assetReference, ILifeTime lifeTime)
            where T : class
        {
            var result = await LoadAssetTaskAsync<GameObject>(assetReference ,lifeTime);
            return result ? 
                result.GetComponent<T>() : 
                null;
        }
        
        #region lifetime
        
        public static AsyncOperationHandle<TAsset> AddTo<TAsset>(this AsyncOperationHandle<TAsset> handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                // if(handle.Result is IDisposable disposable)
                //     disposable.Dispose();
                Addressables.Release(handle);
            });
            return handle;
        }
        
        public static ILifeTime AddTo<TAsset>(this AssetReferenceT<TAsset> handle, ILifeTime lifeTime) 
            where TAsset : Object
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                // if(handle.Asset is IDisposable disposable)
                //     disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        public static ILifeTime AddTo(this AsyncOperationHandle handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                // if(handle.Result is IDisposable disposable)
                //     disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        public static ILifeTime AddTo(this AssetReference handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                // if(handle.Asset is IDisposable disposable)
                //     disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        #endregion
        
        
        private static async UniTask<Object> LoadAssetAsync<TResult>(AssetReference assetReference, ILifeTime lifeTime)
            where TResult : Object
        {
            var result = default(TResult);
            var handle = assetReference.LoadAssetAsync<TResult>();
                
            
            if (handle.Task != null) {
                result = await handle.Task;
                handle.AddTo(lifeTime);
            }

            return result;
        }

    }
}
