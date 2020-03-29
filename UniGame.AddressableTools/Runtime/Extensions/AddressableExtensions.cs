namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::UniCore.Runtime.ProfilerTools;
    using SerializableContext.Runtime.Addressables;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
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
            if(reference.Asset is IDisposable disposable)
                disposable.Dispose();
            
            reference.ReleaseAsset();
        }

        public static async Task<IEnumerable<TSource>> LoadAssetsTaskAsync<TSource, TAsset>(
            this IEnumerable<TAsset> assetReference, 
            List<TSource> resultContainer, ILifeTime lifeTime)
            where TAsset : AssetReference
            where TSource : Object
        {
            return await assetReference.LoadAssetsTaskAsync<TSource, TSource, TAsset>(resultContainer, lifeTime);
        }
        
        public static async Task<IEnumerable<TResult>> LoadAssetsTaskAsync<TSource,TResult,TAsset>(
            this IEnumerable<TAsset> assetReference, 
            IList<TResult> resultContainer, ILifeTime lifeTime)
            where TResult : class
            where TAsset : AssetReference
            where TSource : Object
        {
            var taskList = ClassPool.Spawn<List<Task<TSource>>>();

            foreach (var asset in assetReference) {
                var assetTask = asset.LoadAssetTaskAsync<TSource>(lifeTime);
                taskList.Add(assetTask);
            }

            var result = await Task.WhenAll(taskList);
            for (var j = 0; j < result.Length; j++) {
                if(result[j] is TResult item) resultContainer.Add(item);
            }
            
            taskList.Despawn();

            return resultContainer;
        }
        
        public static async Task<IReadOnlyList<TSource>> LoadAssetsTaskAsync<TSource, TAsset>(
            this IReadOnlyList<TAsset> assetReference, 
            List<TSource> resultContainer, ILifeTime lifeTime)
            where TAsset : AssetReference
            where TSource : Object
        {
            return await assetReference.LoadAssetsTaskAsync<TSource, TSource, TAsset>(resultContainer,lifeTime);
        }

        public static async Task<IReadOnlyList<TResult>> LoadAssetsTaskAsync<TSource,TResult,TAsset>(
            this IReadOnlyList<TAsset> assetReference, 
            List<TResult> resultContainer,ILifeTime lifeTime)
            where TResult : class
            where TAsset : AssetReference
            where TSource : Object
        {
            var taskList = ClassPool.Spawn<List<Task<TSource>>>();
            
            for (var i = 0; i < assetReference.Count; i++) {
                var asset = assetReference[i];
                var assetTask = asset.LoadAssetTaskAsync<TSource>(lifeTime);
                taskList.Add(assetTask);
            }
            
            var result = await Task.WhenAll(taskList);
            for (var j = 0; j < result.Length; j++) {
                if(result[j] is TResult item) resultContainer.Add(item);
            }
            
            taskList.Despawn();

            return resultContainer;
        }
        
        public static async Task<T> LoadAssetTaskAsync<T>(this AssetReference assetReference,ILifeTime lifeTime)
            where T : Object
        {
            if (assetReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return null;
            }

            var result = default(T);
            var dependencies = Addressables.DownloadDependenciesAsync(assetReference.RuntimeKey);
            dependencies.AddTo(lifeTime);
            if(dependencies.Task != null)
                await dependencies.Task;
            
            var handle = assetReference.LoadAssetAsync<T>();
            if (handle.Task != null) {
                result = await handle.Task;
                handle.AddTo(lifeTime);
            }
            
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
            return result != null ? result.GetComponent<T>() : null;
        }

        
        public static async UniTask<T> LoadAssetTaskAsync<T>(
            this AssetReferenceDisposableObject assetReference, 
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
        
        #region lifetime
        
        public static AsyncOperationHandle<TAsset> AddTo<TAsset>(this AsyncOperationHandle<TAsset> handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                if(handle.Result is IDisposable disposable)
                    disposable.Dispose();
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
                if(handle.Asset is IDisposable disposable)
                    disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        public static ILifeTime AddTo(this AsyncOperationHandle handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                if(handle.Result is IDisposable disposable)
                    disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        public static ILifeTime AddTo(this AssetReference handle, ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => {
                if (handle.IsValid() == false)
                    return;
                if(handle.Asset is IDisposable disposable)
                    disposable.Dispose();
                Addressables.Release(handle);
            });
            return lifeTime;
        }
        
        #endregion
    }
}
