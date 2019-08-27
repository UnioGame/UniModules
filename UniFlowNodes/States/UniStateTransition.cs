namespace UniGreenModules.UniNodeSystem.Runtime
{
	using System.Collections;
	using UniCore.Runtime.Interfaces;
	using UniStateMachine.Runtime.Interfaces;
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
}
