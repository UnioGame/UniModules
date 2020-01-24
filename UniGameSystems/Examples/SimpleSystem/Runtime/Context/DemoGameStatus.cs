namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Context
{
    using System;
    using UniRx;

    [Serializable]
    public class DemoGameStatus : IDemoGameStatus
    {
        public BoolReactiveProperty isGameReady = new BoolReactiveProperty(false);

        public IReactiveProperty<bool> IsGameReady => isGameReady;
    }
}
