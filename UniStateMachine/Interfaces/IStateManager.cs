using System;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateManager<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void SetState(TStateType state);
        void Stop();
    }
}