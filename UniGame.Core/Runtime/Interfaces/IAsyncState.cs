namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniRx;

    public interface IAsyncState : IAsyncState<Unit>
    {
    }

    public interface IAsyncState<T,TValue> : 
        IAsyncCommand<T,TValue>, 
        IEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
    }
    
    public interface IAsyncState<T> : 
        IAsyncCommand<T>, 
        IEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
    }
}