using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;


namespace GamePlay.States {

	[CreateAssetMenu(menuName = "States/States/SequenceStateBehaviour", fileName = "SequenceStateBehaviour")]
	public class SequenceStateBehaviour : UniStateBehaviour {
		
		private UniStateBehaviour _activeState;
		
		[SerializeField] 
		private List<UniStateBehaviour> _states = new List<UniStateBehaviour>();

		protected override IEnumerator ExecuteState(IContextProvider context) {

			for (int i = 0; i < _states.Count; i++) {
				
				_activeState = _states[i];
				
				if(!_activeState)
					continue;

				yield return _activeState.Execute(context);
				
				if(_activeState == null)
					continue;
					
				_activeState.Exit(context);
				_activeState = null;
				
			}
			
		}

		protected override void OnExit(IContextProvider context) 
		{
			if (_activeState != null) {
				_activeState.Exit(context);
				_activeState = null;
			}
			base.OnExit(context);
		}
	}
}
