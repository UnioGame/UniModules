
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Tools.StateMachine
{
	
	[CreateAssetMenu(menuName = "UniStateMachine/StateMachine SO",fileName = "StateMachine")]
	public class UniStateMachineObject<TData,TState> : UniStateObject<TData>
	{
		protected ReactiveStateMachine<TData,TState> _stateMachine;

		#region inspector data
		
		[SerializeField]
		private UniStatesSelectorObject<TData> _statesSelectorObject;

		#endregion
		
		#region private methods

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_stateMachine = new ReactiveStateMachine<TData, TState>();
			
			//_stateMachine.Initialize(_statesSelectorObject);
		}

		#endregion
		
	}
}
