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
                if(!activeState)
					continue;

			    _context.AddValue(context, activeState);

                yield return activeState.Execute(context);

                _context.Remove<UniStateBehaviour>(context);

                if (!activeState)
					continue;

			    activeState.Exit(context);

            }
			
		}

		protected override void OnExit(IContext context) 
		{
            Debug.Log("SQU EXIT");
            var state = _context.Get<UniStateBehaviour>(context);
            if (state != null) {
                Debug.Log("SQU STOP");
                state.Exit(context);
			}
			base.OnExit(context);
		}
	}
}
