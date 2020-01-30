namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Context
{
    using System;
    using Runtime.Context;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    [Serializable]
    public class DemoGameContext : 
        IDemoGameContext, 
        IAsyncContextDataSource
    {
        [Header("Current Game session status")]
        public DemoGameStatus gameStatus = new DemoGameStatus();

        public IDemoGameStatus GameStatus => gameStatus;

        public async UniTask<IContext> RegisterAsync(IContext context)
        {
            context.Publish(GameStatus);
            context.Publish<IDemoGameContext>(this);
            return context;
        }

    }
}
