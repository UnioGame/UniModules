using UnityEngine;

namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using Interfaces;
    using Sirenix.OdinInspector;
    using UniRx;

    public abstract class AsyncActorModelInfo<TModel>  : 
        SerializedScriptableObject, IActorInfo<TModel>
        where TModel : IActorModel
    {

        private Subject<TModel> _valueStream = 
            new Subject<TModel>();
		
        public TModel Create()
        {
            var model = CreateDataSource();
            _valueStream.OnNext(model);
            return model;
        }

        public IDisposable Subscribe(IObserver<TModel> observer)
        {
            return _valueStream.Subscribe(observer);
        }
        
        protected abstract TModel CreateDataSource();

    }
}
