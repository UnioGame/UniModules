namespace UniGreenModules.UniNodeSystem.Runtime
{
	using System.Collections;
	using UniCore.Runtime.Interfaces;
	using UniModule.UnityTools.UniStateMachine;
	using UniModule.UnityTools.UniStateMachine.Interfaces;
	using UnityEngine;

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
