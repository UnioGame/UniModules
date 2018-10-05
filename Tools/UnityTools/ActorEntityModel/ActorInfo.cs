using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;

namespace Assets.Tools.UnityTools.ActorEntityModel
{
    [Serializable]
	public abstract class ActorInfo : ScriptableObject, IFactory<ActorModel>
	{
        public string Name;

	    /// <summary>
	    /// behaviour SO
	    /// </summary>
	    public UniStateBehaviour StateObject;

	    /// <summary>
	    /// behaviour component
	    /// </summary>
	    public UniStateComponent StateComponent;

	    public IContextStateBehaviour<IEnumerator> Behaviour =>
	        StateObject ? StateObject  as IContextStateBehaviour<IEnumerator> : StateComponent;

        public abstract ActorModel Create();

	}
}