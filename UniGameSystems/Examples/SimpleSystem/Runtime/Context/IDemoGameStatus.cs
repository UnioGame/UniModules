namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Context
{
    using UniRx;

    public interface IDemoGameStatus
    {
        IReactiveProperty<bool> IsGameReady { get; }
    }
}