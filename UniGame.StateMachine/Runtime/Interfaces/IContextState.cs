namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using global::UniGame.Core.Runtime.ObjectPool;
    using global::UniGame.Core.Runtime;

    public interface IContextState<TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
        bool IsActive { get; }
    }
}
