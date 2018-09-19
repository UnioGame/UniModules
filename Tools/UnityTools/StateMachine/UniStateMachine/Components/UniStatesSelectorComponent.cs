using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{

	public class UniStatesSelectorComponent<TData> : MonoBehaviour , 
		IStateSelector<IStateBehaviour<TData,IEnumerator>>
	{
		[SerializeField]
		private UniStateComponent<TData> _defaultState;
		[SerializeField]
		private List<UniStateSelectorComponent<TData>> _nodes = new List<UniStateSelectorComponent<TData>>();
		
		public IStateBehaviour<TData,IEnumerator> Select()
		{
			if (_nodes == null)
			{
				return _defaultState;
			}

			var node = SelectNode();
			
			return node == null ? _defaultState : node.GetBehaviour();
		}

		private UniStateSelectorComponent<TData> SelectNode()
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