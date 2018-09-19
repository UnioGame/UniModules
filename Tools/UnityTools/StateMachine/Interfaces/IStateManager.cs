using System;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateManager<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void SetState(TStateType state);
    }
}