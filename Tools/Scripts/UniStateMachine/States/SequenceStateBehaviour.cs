using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;


namespace GamePlay.States {

	[CreateAssetMenu(menuName = "States/States/SequenceStateBehaviour", fileName = "SequenceStateBehaviour")]
	public class SequenceStateBehaviour : UniStateBehaviour {
		
		private UniStateBehaviour _activeState;
		
		[SerializeField] 
		private List<UniStateBehaviour> _states = new List<UniStateBehaviour>();

		protected override IEnumerator ExecuteState(IContext context) {

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

		protected override void OnExit(IContext context) 
		{
			if (_activeState != null) {
				_activeState.Exit(context);
				_activeState = null;
			}
			base.OnExit(context);
		}
	}
}
