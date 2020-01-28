namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Context
{
    using System;
    using UniRx;

    [Serializable]
    public class DemoGameStatus : IDemoGameStatus
    {
        public BoolReactiveProperty isGameReadyValue = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsGameReady => isGameReadyValue;

        public IReadOnlyReactiveProperty<bool> SetGameStatus(bool isReady)
        {
            isGameReadyValue.Value = isReady;
            return isGameReadyValue;
        }
    }
}
