using System;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace UniModule.UnityTools.UniStateMachine.Validators
{
	[CreateAssetMenu(menuName = "States/Validators/TimeDelayValidator", fileName = "TimeDelayValidator")]
	public class TimeDelayValidator : UniTransitionValidator 
	{
		[NonSerialized]
		private float _lastValidationTime;
	
		[SerializeField]
		private float DelayBeforeTransition = 0.2f;

		protected override bool ValidateNode(IContext context) 
		{
			var timePassed =  UnityEngine.Time.realtimeSinceStartup - _lastValidationTime;
			_lastValidationTime = UnityEngine.Time.realtimeSinceStartup;
		
			if (timePassed < DelayBeforeTransition || _lastValidationTime<=0) {
				return false;
			}

			//reset time
			_lastValidationTime = 0f;
		
			return true;
		
		}
	}
}
