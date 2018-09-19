
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Tools.StateMachine
{
	[Serializable]
	public class UniStateMachineComponent<TData> : UniStateComponent<TData>
	{
		protected ReactiveStateMachine<IStateBehaviour<TData,IEnumerator>> _stateMachine;

		#region inspector data
		
		[SerializeField]
		private UniStatesSelectorComponent<TData> _statesSelectorObject;

		#endregion
		
		
		#region private methods

		protected override void OnInitialize()
		{
			_stateMachine = new ReactiveStateMachine<IStateBehaviour<TData,IEnumerator>>();
		}

		protected override IEnumerator UpdateState(TData data)
		{
			yield return _stateMachine.Execute();
		}

		#endregion
		
	}
}
