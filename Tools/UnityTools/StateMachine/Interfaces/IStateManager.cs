using System;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IStateManager<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void SetState(TStateType state);
        void Stop();
    }
}