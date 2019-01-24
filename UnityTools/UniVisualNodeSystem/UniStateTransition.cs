using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Interfaces;
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
