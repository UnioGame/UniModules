using System;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateController<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        IObservable<TStateType> StateObservable { get; }

        void SetState(TStateType state);
    }
}