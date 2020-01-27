namespace UniGreenModules.UniGame.AddressableTools.Runtime.Extensions
{
    using UniCore.Runtime.ProfilerTools;
    using UniRx.Async;
    using UnityEngine.AddressableAssets;

    public static class AddressableExtensions
    {
        public static async UniTask<T> LoadTaskAsync<T>(this T assetReference)
            where T : AssetReference
        {
            if (assetReference.RuntimeKey == null) {
                GameLog.LogError($"AssetReference key is NULL {assetReference}");
                return null;
            }
            
            var handler = assetReference.LoadAssetAsync<T>();
            return await handler.Task;
        }
    }
}
