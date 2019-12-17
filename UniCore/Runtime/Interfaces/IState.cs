namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IState : 
        ICommand, 
        IEndPoint,
        ILifeTimeContext,
        IPoolable,
        IActiveStatus
    {
    }
}