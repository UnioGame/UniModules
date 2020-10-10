namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System;

    public interface IStateController<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        IObservable<TStateType> StateObservable { get; }

        void SetState(TStateType state);
    }
}