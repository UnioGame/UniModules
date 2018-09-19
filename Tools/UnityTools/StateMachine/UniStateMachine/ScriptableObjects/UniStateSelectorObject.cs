using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class UniStateSelectorObject<TData> : ScriptableObject
{
	[SerializeField]
	private UniStateObject<TData> _stateObject;
	
	public int Priority { get; }

	public virtual bool IsReady()
	{
		return false;
	}

	public virtual IStateBehaviour<TData,IEnumerator> GetBehaviour()
	{
		return _stateObject;
	}	
	
}
