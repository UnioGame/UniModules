namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using System.Collections.Generic;
    using SerializableContext.Runtime.Addressables;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ProfilerTools;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public static class AddressableExtensions
    {
        public static async UniTask<SceneInstance> LoadSceneTaskAsync(
            this AssetReference sceneReference,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            int priority = 100)
        {
            if (sceneReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {sceneReference}");
                return default;
            }

            var scene = await sceneReference.LoadSceneAsync(loadSceneMode, activateOnLoad, priority).Task;
            return scene;
        }

        public static async UniTask<IReadOnlyList<TSource>> LoadAssetsTaskAsync<TSource, TAsset>(this IReadOnlyList<TAsset> assetReference, List<TSource> resultContainer)
            where TAsset : AssetReference
            where TSource : Object
        {
            return await assetReference.LoadAssetsTaskAsync<TSource, TSource, TAsset>(resultContainer);
        }

        public static async UniTask<IReadOnlyList<TResult>> LoadAssetsTaskAsync<TSource,TResult,TAsset>(this IReadOnlyList<TAsset> assetReference, List<TResult> resultContainer)
            where TResult : class
            where TAsset : AssetReference
            where TSource : Object
        {
            var taskList = ClassPool.Spawn<List<UniTask<TSource>>>();
            
            for (var i = 0; i < assetReference.Count; i++) {
                var asset = assetReference[i];
                var assetTask = asset.LoadAssetTaskAsync<TSource>();
                taskList.Add(assetTask);
            }
            
            var result = await UniTask.WhenAll(taskList);
            for (var j = 0; j < result.Length; j++) {
                if(result[j] is TResult item) resultContainer.Add(item);
            }
            
            taskList.DespawnCollection();

            return resultContainer;
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(this AssetReference assetReference)
            where T : class
        {
            if (assetReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return null;
            }
            
            var handler = assetReference.LoadAssetAsync<T>();
            return await handler.Task;
        }
        
        public static async UniTask<(TAsset,TResult)> LoadAssetTaskAsync<TAsset,TResult>(this AssetReference assetReference)
            where TAsset : Object
            where TResult : class
        {
            if (assetReference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return (null,null);
            }
            
            var handler = assetReference.LoadAssetAsync<TAsset>();
            var result = await handler.Task;
            return (result,result as TResult);
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(this ScriptableObjectAssetReference assetReference)
            where T : class
        {
            var result = await LoadAssetTaskAsync<ScriptableObject>(assetReference as AssetReference);
            return result as T;
        }
        
        public static async UniTask<T> LoadAssetTaskAsync<T>(this AssetReferenceT<T> assetReference)
            where T : Object
        {
            return await LoadAssetTaskAsync<T>(assetReference as AssetReference);
        }
        
        
    }
}
