using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{

	[Serializable]
	public class UniTransitionValidator :ScriptableObject, IValidator<IContextProvider>
	{
		[HideInInspector]
		[SerializeField] 
		private List<UniTransitionValidator> _validators = new List<UniTransitionValidator>();
		
		public List<UniTransitionValidator> Validators => _validators;

		public bool Validate(IContextProvider context)
		{
			for (var i = 0; i < _validators.Count; i++)
			{
				var validator = _validators[i];
				if (validator == null)
				{
					Debug.LogErrorFormat("Validator {0} item with index {1} is NULl", this, i);
				}

				if (_validators[i].Validate(context) == false)
					return false;
			}

			return ValidateNode(context);
		}

		protected virtual bool ValidateNode(IContextProvider context)
		{
			return true;
		}
	}
}