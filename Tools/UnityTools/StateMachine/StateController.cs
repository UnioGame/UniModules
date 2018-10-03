using System;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx;

namespace Assets.Tools.UnityTools.StateMachine
{
    public class StateController<TState> : IStateController<TState>
    {
        private ReactiveProperty<TState> _stateValue;

        public StateController()
        {
            _stateValue = new ReactiveProperty<TState>();
        }

        #region public properties
        
        public TState CurrentState { get; private set; }
        public TState PreviousState { get; private set; }
        public IObservable<TState> StateObservable { get { return _stateValue; } }

        #endregion

        public void SetState(TState state)
        { 
            PreviousState = CurrentState;
            CurrentState = state;
            _stateValue.SetValueAndForceNotify(state);
        }

        public void Dispose()
        {
            _stateValue.Dispose();
        }
    }
}
