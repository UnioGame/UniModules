using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UnityTools.ActorEntityModel
{
    [Serializable]
	public abstract class ActorInfo : ScriptableObject, IFactory<ActorModel>
	{
        #region inspector

        [SerializeField]
        protected string _name;

	    [SerializeField]
	    protected UniNodesGraph _statesGraph;

	    /// <summary>
	    /// behaviour SO
	    /// </summary>
	    [SerializeField]
	    protected UniNode _stateObject;

        #endregion

        public string InfoName => _name;
        
	    public IContextState<IEnumerator> Behaviour => 
	        _statesGraph ? (IContextState<IEnumerator>)_statesGraph : _stateObject;

        public abstract ActorModel Create();

	}
}