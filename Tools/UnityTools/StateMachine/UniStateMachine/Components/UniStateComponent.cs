using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{
	[Serializable]
	public class UniStateComponent<TData> : MonoBehaviour, IStateBehaviour<TData,IEnumerator>
	{
		[NonSerialized]
		private StateFunctionBehaviour<TData> _stateFunctionBehaviour = new StateFunctionBehaviour<TData>();

		public bool IsActive
		{
			get { return _stateFunctionBehaviour.IsActive; }
		}

		#region public methods
		
		public IEnumerator Execute(TData data)
		{
			_stateFunctionBehaviour.Initialize(UpdateState);
			yield return _stateFunctionBehaviour.Execute(data);
			
		}

		public virtual void Exit()
		{
			if(_stateFunctionBehaviour!=null)
				_stateFunctionBehaviour.Exit();
			OnStop();
		}

		#endregion


		protected virtual IEnumerator UpdateState(TData data)
		{
			yield break;
		}
		
		private void Initialize()
		{
			OnInitialize();
		}

		protected virtual void OnInitialize()
		{
			
		}

		protected virtual void OnStop()
		{
			
		}

		private void OnDestroy() {
			Exit();
		}
	}
}
