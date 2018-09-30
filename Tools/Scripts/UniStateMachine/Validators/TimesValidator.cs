using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Validators/TimesValidator", fileName = "TimesValidator")]
public class TimesValidator : UniTransitionValidator
{
	[SerializeField]
	private int AllowTransitionCount = 1;
	[SerializeField]
	private bool Unlimited = false;

	[NonSerialized]
	private int _counter;

	protected override bool ValidateNode(IContextProvider context)
	{
		var result = Unlimited || _counter < AllowTransitionCount;
		_counter++;
		return result;
	}
}
