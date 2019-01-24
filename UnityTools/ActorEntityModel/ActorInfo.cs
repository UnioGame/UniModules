using System;
using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.ActorEntityModel
{
    [Serializable]
	public abstract class ActorInfo : 
		ScriptableObject, IFactory<ActorModel>
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

        public IContextState<IEnumerator> Behaviour => _statesGraph ? _statesGraph : 
	        (IContextState<IEnumerator>)_stateObject;


        public ActorModel Create()
        {
	        var model = CreateModel();
	        model.Behaviour = Behaviour;
	        return model;
        }

        protected abstract ActorModel CreateModel();

	}
}