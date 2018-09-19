using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class UniStateSelectorComponent : MonoBehaviour
{
	[SerializeField]
	private UniStateComponent _stateObject;
	
	public int Priority { get; }

	public virtual bool IsReady()
	{
		return false;
	}

	public virtual IStateBehaviour<IEnumerator> GetBehaviour()
	{
		return _stateObject;
	}	
	
}
