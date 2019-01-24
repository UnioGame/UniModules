using System;
using System.Collections;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;
using UniModule.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine {

	public class ProxyStateBehaviour : ContextStateBehaviour {

		private Func<IContext,IEnumerator> _updateFunction;
	    private Action<IContextData<IContext>> _onInitialize;
		private Action<IContext> _onExit;
		private Action<IContext> _onPostExecute;

        /// <summary>
        /// set state functions
        /// </summary>
        /// <param name="updateFunction"></param>
        /// <param name="onInitialize">initialize action</param>
        /// <param name="onExit"></param>
		public void Initialize(Func<IContext, IEnumerator> updateFunction,
		    Action<IContextData<IContext>> onInitialize = null,
		    Action<IContext> onExit = null,
	        Action<IContext> onPostExecute = null)
		{
			_updateFunction = updateFunction;
		    _onInitialize = onInitialize;
			_onExit = onExit;
			_onPostExecute = onPostExecute;
			
			Initialize();
		}

		protected override void OnInitialize(IContextData<IContext> stateContext) {
		    _onInitialize?.Invoke(stateContext);
		}

	    public override void Dispose()
	    {
	        _updateFunction = null;
	        _onInitialize = null;
	        _onExit = null;
		    _onPostExecute = null;
		    
		    base.Dispose();
	    }

	    protected override void OnExit(IContext context) {
			_onExit?.Invoke(context);
		}

		protected override void OnPostExecute(IContext context) 
		{
			_onPostExecute?.Invoke(context);
		}

		protected override IEnumerator ExecuteState(IContext context)
		{
			if (_updateFunction != null) {
				yield return _updateFunction(context);
			}
		}

	}
}
