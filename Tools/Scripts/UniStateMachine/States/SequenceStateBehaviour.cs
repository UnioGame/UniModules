using System.Collections;
using System.Collections.Generic;
using UniStateMachine;
using UnityEngine;


namespace GamePlay.States {

	[CreateAssetMenu(menuName = "States/States/SequenceStateBehaviour", fileName = "SequenceStateBehaviour")]
	public class SequenceStateBehaviour : UniStateBehaviour {
		
		private UniStateBehaviour _activeState;
		
		[SerializeField] 
		private List<UniStateBehaviour> _states = new List<UniStateBehaviour>();

		protected override IEnumerator ExecuteState() {

			for (int i = 0; i < _states.Count; i++) {
				
				_activeState = _states[i];
				
				if(!_activeState)
					continue;
				
				_activeState.Initialize(_contextProvider);
				
				yield return _activeState.Execute();
				
				if(_activeState == null)
					continue;
					
				_activeState.Exit();
				_activeState = null;
				
			}
			
		}

		protected override void OnExit() 
		{
			if (_activeState != null) {
				_activeState.Exit();
				_activeState = null;
			}
			base.OnExit();
		}
	}
}
