using System;

namespace UniStateMachine
{
    public interface IStateManager<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void SetState(TStateType state);
        void Stop();
    }
}