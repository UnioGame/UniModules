namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using SerializableContext.Runtime.Addressables;
    using UniCore.Runtime.ProfilerTools;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public static class AddressableExtensions
    {
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
