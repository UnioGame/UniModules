namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IState<out TAwaiter> : 
        IStateBehaviour<TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
    }

    public interface IState : 
        ICommand, 
        IEndPoint,
        ILifeTimeContext,
        IPoolable,
        IActiveStatus
    {
    }
}