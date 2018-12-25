using Assets.Tools.UnityTools.ProfilerTools;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{
    public class AsyncFiniteStateMachine : IAsyncStateMachine
    {
        #region private properties

        private Color _color = Color.blue;
        private Color _stopColor = Color.green;
        private const string ExecuteStateTemplate = "Execute State = {0}";
        private const string StopStateTemplate = "STOP State = {0}";
        private readonly IAsyncStateExecutor _executor;

        #endregion

        #region public properties

        public IAsyncStateBehaviour State { get; protected set; }

        #endregion

        #region constructor

        public AsyncFiniteStateMachine(IAsyncStateExecutor executor)
        {
            _executor = executor;
        }

        #endregion

        #region public methods

        public void Execute(IAsyncStateBehaviour state)
        {
            GameProfiler.BeginSample("Stop state");
            Stop();
            GameProfiler.EndSample();
            if (state == null)
            {
                Debug.LogErrorFormat("State Machine State NULL not exists in current context");
                return;
            }
            GameLog.LogFormat(ExecuteStateTemplate, _color, state);
            GameProfiler.BeginSample("ExecuteState");
            ExecuteState(state);
            GameProfiler.EndSample();
        }

        public virtual void Stop()
        {
            if (State != null)
            {
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


        protected virtual void ExecuteState(IAsyncStateBehaviour stateBehaviour)
        {
            State = stateBehaviour;
            _executor.Execute(State.Execute());
        }

        #endregion
    }
}
