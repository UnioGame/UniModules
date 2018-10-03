using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old
{
    public interface IAssetOperation : IResetable, ICommandRoutine
    {
        string Error { get; }
        string AssetName { get; }
        bool IsDone { get; }
    }
}