namespace UniModule.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IAssetOperation : IResetable, ICommandRoutine
    {
        string Error { get; }
        string AssetName { get; }
        bool IsDone { get; }
    }
}