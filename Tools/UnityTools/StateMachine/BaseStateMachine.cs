using System.Diagnostics;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Tools.StateMachine
{
    public class BaseStateMachine<TState> :
        IStateMachine<TState>
    {
        #region private properties

        private static Color _color = Color.blue;
        private static Color _stopColor = Color.green;
        private const string ExecuteStateTemplate = "Execute State = {0}";
        private const string StopStateTemplate = "STOP State = {0}";
        
        private readonly IStateExecutor<TState> _executor;

        #endregion

        #region public properties

        public TState State { get; protected set; }

        #endregion

        #region constructor

        public BaseStateMachine(IStateExecutor<TState> executor)
        {
            _executor = executor;
        }

        #endregion

        #region public methods

        public void Execute(TState state)
        {
            GameLog.LogGameState(string.Format("STATE : move from {0} to state {1}",State,state));
            GameProfiler.BeginSample("Stop state");
            Stop();
            GameProfiler.EndSample();
            
            State = state;
            StateEnter(State);
            
            GameProfiler.BeginSample("ExecuteState");
            _executor.Execute(state);
            GameProfiler.EndSample();
        }

        public void Stop()
        {
            GameLog.LogFormat(StopStateTemplate, _stopColor, State.GetType().Name);
            
            //stop state
            _executor.Stop();
            
            //on exit action
            OnExit();
        }

        #endregion

        #region private methods

        protected virtual void StateEnter(TState state)
        {
            
        }

        protected virtual void OnExit() {
            
        }

        #endregion
    }
}
