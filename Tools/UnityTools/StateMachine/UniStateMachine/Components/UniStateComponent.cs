using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{
	public class UniStateComponent : MonoBehaviour, IStateBehaviour<IEnumerator>
	{
		[NonSerialized]
		private bool _initialized = false;
		[NonSerialized]
		private StateFunctionBehaviour _stateFunctionBehaviour;
		
		#region public methods
		
		public IEnumerator Execute()
		{
			if (_initialized == false)
			{
				_initialized = true;
				Initialize();
			}
			
			_stateFunctionBehaviour.Initialize(UpdateState);
			yield return _stateFunctionBehaviour.Execute();
			
		}

		public virtual void Stop()
		{
			_stateFunctionBehaviour.Stop();
			OnStop();
		}

		#endregion


		protected virtual IEnumerator UpdateState()
		{
			yield break;
		}
		
		private void Initialize()
		{
			_stateFunctionBehaviour = new StateFunctionBehaviour();
			OnInitialize();
		}

		protected virtual void OnInitialize()
		{
			
		}

		protected virtual void OnStop()
		{
			
		}
	}
}
