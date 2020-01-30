namespace UniGreenModules.UniGameSystem.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IGameService : IDisposable, ILifeTimeContext
    {

        /// <summary>
        /// is service ready to work
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsReady { get; }
        
        /// <summary>
        /// Bind to target context during lifetime
        /// if lifetime is null, use lifetime of context
        /// </summary>
        /// <param name="context">data context</param>
        /// <param name="lifeTime">lifetime object</param>
        /// <returns>service context</returns>
        IContext Bind(IContext context, ILifeTime lifeTime = null);

    }
}
