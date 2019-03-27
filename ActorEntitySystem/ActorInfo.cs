using System;
using System.Collections;
using Sirenix.OdinInspector;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.ActorEntityModel
{
    [Serializable]
	public abstract class ActorInfo : 
		SerializedScriptableObject, 
		IActorInfo<IActorModel> 
	{

		private Subject<IActorModel> _valueStream = 
			new Subject<IActorModel>();
		
		public IActorModel Create()
        {
	        var model = CreateDataSource();
	        _valueStream.OnNext(model);
	        return model;
        }

        public IDisposable Subscribe(IObserver<IActorModel> observer)
        {
	        return _valueStream.Subscribe(observer);
        }
        
        protected abstract IActorModel CreateDataSource();

	}
}