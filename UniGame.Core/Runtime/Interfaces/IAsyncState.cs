namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IAsyncState : IAsyncState<Unit>
    {
    }

    public interface IAsyncState<T> : 
        IAsyncCommand<T>, 
        IAsyncEndPoint<T>,
        ILifeTimeContext,
        IActiveStatus
    {
    }
}