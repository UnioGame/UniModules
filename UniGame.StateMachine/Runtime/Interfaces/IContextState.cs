namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IContextState<TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
        bool IsActive { get; }
    }
}
