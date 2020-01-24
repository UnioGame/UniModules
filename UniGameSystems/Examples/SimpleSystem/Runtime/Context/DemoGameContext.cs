namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Context
{
    using System;
    using Runtime.Context;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    [Serializable]
    public class DemoGameContext : 
        IDemoGameContext, 
        IContextDataSource
    {
        [Header("Current Game session status")]
        public DemoGameStatus gameStatus;

        public IDemoGameStatus GameStatus => gameStatus;

        public void Register(IContext context)
        {
            context.Publish(GameStatus);
            context.Publish<IDemoGameContext>(this);
        }
    }
}
