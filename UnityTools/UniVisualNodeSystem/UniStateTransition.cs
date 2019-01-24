using System.Collections;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{
	public class UniStateTransition : ScriptableObject , IUniStateTransition
	{
		[SerializeField]
		protected bool _defaultValidatorValue = false;

		public virtual bool Validate(IContext data)
		{
			return _defaultValidatorValue;
		}

		public virtual IContextState<IEnumerator> SelectState(IContext context)
		{
			return null;
		}
	}
	
	public class UniStateValidator : ScriptableObject , IValidator<IContext>
	{
		[SerializeField]
		protected bool _defaultValidatorValue = false;


		public virtual bool Validate(IContext data)
		{
			return _defaultValidatorValue;
		}

	}
}
