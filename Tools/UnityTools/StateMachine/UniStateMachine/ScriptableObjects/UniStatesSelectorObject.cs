using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine
{

	[CreateAssetMenu(menuName = "UniStateMachine/StatesSelector SO",fileName = "StatesSelector")]
	public class UniStatesSelectorObject<TData> : ScriptableObject , 
		IStateSelector<IStateBehaviour<TData,IEnumerator>>
	{
		[SerializeField]
		private UniStateObject<TData> _defaultState;
		[SerializeField]
		private List<UniStateSelectorObject<TData>> _nodes = new List<UniStateSelectorObject<TData>>();
		
		public IStateBehaviour<TData,IEnumerator> Select()
		{
			if (_nodes == null)
			{
				return _defaultState;
			}

			var node = SelectNode();
			
			return node == null ? _defaultState : node.GetBehaviour();
		}

		private UniStateSelectorObject<TData> SelectNode()
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