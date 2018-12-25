using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ReorderableInspector;
using Assets.Tools.UnityTools.StateMachine;
using UnityEngine;

namespace UniStateMachine
{

	[Serializable]
	[CreateAssetMenu(menuName = "UniStateMachine/Validators/DefaultValidator", fileName = "DefaultValidator")]
	public class UniTransitionValidator :ScriptableObject, IValidator<IContext>
	{
		[SerializeField]
		protected bool _defaultValue = true;
		
		[Reorderable]
		[SerializeField] 
		private List<UniTransitionValidator> _validators = new List<UniTransitionValidator>();
		
		public List<UniTransitionValidator> Validators => _validators;

		public bool Validate(IContext context)
		{
			var result = ValidateNode(context);
			
			for (var i = 0; result && i < _validators.Count; i++)
			{
				var validator = _validators[i];
				if (validator == null)
				{
					Debug.LogErrorFormat("Validator {0} item with index {1} is NULl", this, i);
				}

				if (_validators[i].Validate(context) == false)
					return false;
			}

			StateLogger.LogValidator("VALIDATOR {0} VALUE {1}",this.name,result);
			
			return result;
		}

		protected virtual bool ValidateNode(IContext context)
		{
			return _defaultValue;
		}
	}
}