
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Tools.StateMachine
{
	public class UniStateMachineComponent : UniStateComponent
	{
		protected ReactiveStateMachine<IEnumerator> _stateMachine;

		#region inspector data
		
		[SerializeField]
		private UniStatesSelectorComponent _statesSelectorObject;

		#endregion
		
		
		#region private methods

		protected override void OnInitialize()
		{
			_stateMachine = new RxRoutineStateMachine(_statesSelectorObject);
		}

		protected override IEnumerator UpdateState()
		{
			yield return _stateMachine.Execute();
		}

		#endregion
		
	}
}
