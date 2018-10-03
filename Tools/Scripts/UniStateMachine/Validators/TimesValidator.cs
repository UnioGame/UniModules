using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
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

	protected override bool ValidateNode(IContext context)
	{
		var result = Unlimited || _counter < AllowTransitionCount;
		_counter++;
		return result;
	}
}
