namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System;

    public interface IStateManager<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void SetState(TStateType state);
        void Stop();
    }
}