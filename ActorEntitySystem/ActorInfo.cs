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
		ScriptableObject, IFactory<IActorModel>
	{

		public IActorModel Create()
        {
	        var model = CreateDataSource();
	        return model;
        }

        protected abstract IActorModel CreateDataSource();

	}
}