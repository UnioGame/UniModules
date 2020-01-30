namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Context
{
    using UniRx;

    public interface IImmutableDemoGameStatus
    {
        IReadOnlyReactiveProperty<bool> IsGameReady { get; }
    }
}