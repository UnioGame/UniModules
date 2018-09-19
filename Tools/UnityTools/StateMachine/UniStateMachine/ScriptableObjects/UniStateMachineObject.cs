
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Tools.StateMachine
{
	
	[CreateAssetMenu(menuName = "UniStateMachine/StateMachine SO",fileName = "StateMachine")]
	public class UniStateMachineObject : UniStateObject
	{
		protected ReactiveStateMachine<IEnumerator> _stateMachine;

		#region inspector data
		
		[SerializeField]
		private UniStatesSelectorObject _statesSelectorObject;

		#endregion
		
		
		#region private methods

		protected override void OnInitialize()
		{
			_stateMachine = new RxRoutineStateMachine(_statesSelectorObject);
		}

		#endregion
		
	}
}
