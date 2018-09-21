using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{
	[Serializable]
	public class UniSelectorNode : UniNodeValidator
	{
		[SerializeField] 
		private UniStateBehaviour _stateBehaviour;

		public void SetBehaviour(UniStateBehaviour behaviour)
		{
			_stateBehaviour = behaviour;
		}
		
		public virtual UniStateBehaviour GetState()
		{
			return _stateBehaviour;
		}

	}

}
