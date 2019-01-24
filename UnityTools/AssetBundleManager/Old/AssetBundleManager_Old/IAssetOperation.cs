using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old
{
    public interface IAssetOperation : IResetable, ICommandRoutine
    {
        string Error { get; }
        string AssetName { get; }
        bool IsDone { get; }
    }
}