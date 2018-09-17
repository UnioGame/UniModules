using System;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateController<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        IObservable<TStateType> StateObservable { get; }

        void SetState(TStateType state);
    }
}