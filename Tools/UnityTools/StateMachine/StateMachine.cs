using System.Diagnostics;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateMachine<TAwaiter> :
        IStateMachine<IStateBehaviour<TAwaiter>>
    {
        #region private properties

        private Color _color = Color.blue;
        private Color _stopColor = Color.green;
        private const string ExecuteStateTemplate = "Execute State = {0}";
        private const string StopStateTemplate = "STOP State = {0}";
        private readonly IStateExecutor<TAwaiter> _executor;

        #endregion

        #region public properties

        public IStateBehaviour<TAwaiter> State { get; protected set; }

        #endregion

        #region constructor

        public StateMachine(IStateExecutor<TAwaiter> executor)
        {
            _executor = executor;
        }

        #endregion

        #region public methods

        public void Execute(IStateBehaviour<TAwaiter> state)
        {
            GameLog.LogGameState(string.Format("STATE : move from {0} to state {1}",State,state));

            GameProfiler.BeginSample("Stop state");
            Stop();
            GameProfiler.EndSample();

            if (state == null)
            {
                Debug.LogErrorFormat("State Machine State NULL not exists in current context");
                return;
            }

            GameProfiler.BeginSample("ExecuteState");
            ExecuteState(state);
            GameProfiler.EndSample();
        }

        public virtual void Stop()
        {
            if (State != null)
            {
                State.Stop();
                GameLog.LogFormat(StopStateTemplate, _stopColor, State.GetType().Name);
                State = null;
            }
            _executor.Stop();
        }

        public void Dispose()
        {
            Stop();
            State = null;
        }

        #endregion

        #region private methods

        protected virtual void ExecuteState(IStateBehaviour<TAwaiter> state)
        {
            State = state;
            _executor.Execute(State.Execute());
        }

        #endregion
    }
}
