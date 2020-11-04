
namespace UniModules.UniStateMachine.Runtime.ContextStateMachine
{
    using System;
    using System.Collections;
    using UniGame.Core.Runtime.Interfaces;

    public class ProxyState : ContextState
    {
        private Func<IContext, IEnumerator> _updateFunction;
        private Action<IContext> _onInitialize;
        private Action<IContext> _onExit;
        private Action<IContext> _onPostExecute;

        /// <summary>
        /// set state functions
        /// </summary>
        /// <param name="updateFunction">update operation</param>
        /// <param name="onInitialize">initialize action</param>
        /// <param name="onExit">on execution exit operation</param>
        /// <param name="onPostExecute">operation before execution completed</param>
        public void Initialize(Func<IContext, IEnumerator> updateFunction,
            Action<IContext> onInitialize = null,
            Action<IContext> onExit = null,
            Action<IContext> onPostExecute = null)
        {
            Release();

            _updateFunction = updateFunction;
            _onInitialize = onInitialize;
            _onExit = onExit;
            _onPostExecute = onPostExecute;

            LifeTime.AddCleanUpAction(CleanUpOperations);
        }

        protected override void OnInitialize(IContext stateContext)
        {
            _onInitialize?.Invoke(stateContext);
        }

        protected override void OnExit(IContext context)
        {
            _onExit?.Invoke(context);
        }

        protected override void OnPostExecute(IContext context)
        {
            _onPostExecute?.Invoke(context);
        }

        protected override IEnumerator ExecuteState(IContext context)
        {
            if (_updateFunction != null)
            {
                yield return _updateFunction(context);
            }
        }

        private void CleanUpOperations()
        {
            _updateFunction = null;
            _onInitialize = null;
            _onExit = null;
            _onPostExecute = null;
        }
    }
}