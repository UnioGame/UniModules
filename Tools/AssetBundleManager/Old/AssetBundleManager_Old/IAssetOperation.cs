namespace AssetBundlesModule_Old
{
    public interface IAssetOperation : IResetable, ICommandRoutine
    {
        string Error { get; }
        string AssetName { get; }
        bool IsDone { get; }
    }
}