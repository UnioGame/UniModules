namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using UniCore.Runtime.ProfilerTools;
    using UniRx.Async;
    using UnityEngine.AddressableAssets;

    public static class AddressableExtensions
    {
        public static async UniTask<T> LoadAssetTaskAsync<T>(this AssetReference assetReference)
            where T : class
        {
            if (string.IsNullOrEmpty(assetReference.AssetGUID)) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return null;
            }
            
            var handler = assetReference.LoadAssetAsync<T>();
            return await handler.Task.AsUniTask();
        }
    }
}
