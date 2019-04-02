using System;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace UniModule.UnityTools.UniStateMachine.Validators
{
	[CreateAssetMenu(menuName = "States/Validators/TimesValidator", fileName = "TimesValidator")]
	public class TimesValidator : UniTransitionValidator
	{
		[SerializeField]
		private int AllowedTransitionCount = 1;
		[SerializeField]
		private bool Unlimited = false;

		[NonSerialized]
		private Dictionary<int, int> _transitionCounter;
	
		protected override bool ValidateNode(IContext context) {

			if (_transitionCounter == null) {
				_transitionCounter = new Dictionary<int, int>();
			}
		
			var id = context.GetHashCode();
			var counter = 0;
			_transitionCounter.TryGetValue(id, out counter);
		
			var result = Unlimited || counter < AllowedTransitionCount;
			counter++;
			_transitionCounter[id] = counter;
		
			return result;
		} 
	}
}
