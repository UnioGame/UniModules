using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{

	public class UniStatesSelectorComponent : MonoBehaviour , IStateSelector<IStateBehaviour<IEnumerator>>
	{
		[SerializeField]
		private UniStateComponent _defaultState;
		[SerializeField]
		private List<UniStateSelectorComponent> _nodes = new List<UniStateSelectorComponent>();
		
		public IStateBehaviour<IEnumerator> Select()
		{
			if (_nodes == null)
			{
				return _defaultState;
			}

			var node = SelectNode();
			
			return node == null ? _defaultState : node.GetBehaviour();
		}

		private UniStateSelectorComponent SelectNode()
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				var node = _nodes[i];
				if (node.IsReady())
					return node;
			}

			return null;
		}
	}
	
}