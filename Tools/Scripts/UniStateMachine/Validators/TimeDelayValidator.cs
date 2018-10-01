using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Validators/TimeDelayValidator", fileName = "TimeDelayValidator")]
public class TimeDelayValidator : UniTransitionValidator 
{
	[NonSerialized]
	private float _lastValidationTime;
	
	[SerializeField]
	private float DelayBeforeTransition = 0.2f;

	protected override bool ValidateNode(IContextProvider context) 
	{
		var timePassed =  Time.realtimeSinceStartup - _lastValidationTime;
		_lastValidationTime = Time.realtimeSinceStartup;
		
		if (timePassed < DelayBeforeTransition || _lastValidationTime<=0) {
			return false;
		}

		//reset time
		_lastValidationTime = 0f;
		
		return true;
		
	}
}
