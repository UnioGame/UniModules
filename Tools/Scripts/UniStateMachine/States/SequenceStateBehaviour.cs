using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;


namespace GamePlay.States {

	[CreateAssetMenu(menuName = "States/States/SequenceStateBehaviour", fileName = "SequenceStateBehaviour")]
	public class SequenceStateBehaviour : UniStateBehaviour {
		
		[SerializeField] 
		private List<UniStateBehaviour> _states = new List<UniStateBehaviour>();

		protected override IEnumerator ExecuteState(IContext context) {

			for (int i = 0; i < _states.Count; i++) {
				
				var activeState = _states[i];
				_stateContext.AddValue(context, activeState);

				if(!activeState)
					continue;

				yield return activeState.Execute(context);
				
				if(activeState == null)
					continue;

			    activeState.Exit(context);
			    _stateContext.Remove<UniStateBehaviour>(context);

            }
			
		}

		protected override void OnExit(IContext context) 
		{
            Debug.Log("SQU EXIT");
            var state = _stateContext.Get<UniStateBehaviour>(context);
            if (state != null) {
                state.Exit(context);
			}
			base.OnExit(context);
		}
	}
}
