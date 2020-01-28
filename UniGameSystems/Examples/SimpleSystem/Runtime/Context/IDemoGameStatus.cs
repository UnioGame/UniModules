namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Context
{
    using UniRx;

    public interface IDemoGameStatus : IImmutableDemoGameStatus
    {
        IReadOnlyReactiveProperty<bool> SetGameStatus(bool isReady);

    }
}