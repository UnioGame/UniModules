namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IState<TAwaiter> : 
        IStateBehaviour<TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
    }
}