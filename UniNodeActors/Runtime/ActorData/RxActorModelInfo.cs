using System;
using System.Threading.Tasks;
using UniGreenModules.UniNodeActors.Runtime.Interfaces;
using UniRx;
using UnityEngine;

namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    public abstract class RxActorModelInfo<TModel>  : 
        ScriptableObject, IRxActorInfo<TModel>
        where TModel : IActorModel
    {

        private Subject<TModel> _valueStream = 
            new Subject<TModel>();
		
        public IObservable<TModel> Create()
        {
            var observable = CreateDataSource();
            observable.Subscribe(_valueStream);
            return observable;
        }

        public IDisposable Subscribe(IObserver<TModel> observer)
        {
            return _valueStream.Subscribe(observer);
        }
        
        protected abstract IObservable<TModel> CreateDataSource();

    }
}