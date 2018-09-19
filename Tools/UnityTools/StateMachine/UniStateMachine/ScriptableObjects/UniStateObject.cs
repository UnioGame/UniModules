using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{
	public class UniStateObject<TData> : ScriptableObjectRoutine<TData,IEnumerator>, 
		IStateBehaviour<TData,IEnumerator>
	{

		[NonSerialized]
		private StateFunctionBehaviour<TData> _stateFunctionBehaviour;
		
		#region public methods

		public void Exit()
		{
			_stateFunctionBehaviour.Exit();
		}

		#endregion


		public bool IsActive => _stateFunctionBehaviour != null && 
		                        _stateFunctionBehaviour.IsActive;

		#region private methods
		
		protected override IEnumerator OnExecute(TData data) {
			
			yield return _stateFunctionBehaviour.Execute(data);
			
		}

		protected override void OnInitialize() {
			
			_stateFunctionBehaviour = new StateFunctionBehaviour<TData>();
			_stateFunctionBehaviour.Initialize(UpdateState,null,OnStop);
			base.OnInitialize();
			
		}

		protected virtual void OnStop(){}
		
		protected virtual IEnumerator UpdateState(TData data){yield break;}

		#endregion
	}
}
