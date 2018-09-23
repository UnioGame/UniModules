using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace UniStateMachine
{

	[Serializable]
	public class UniNodeValidator :ScriptableObject, IValidator<IContextProvider>
	{

		[SerializeField] 
		private List<UniNodeValidator> _validators = new List<UniNodeValidator>();

		public List<UniNodeValidator> Validators => _validators;

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