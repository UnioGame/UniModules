using UnityEngine;

namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using UniRx;

    public abstract class AsyncActorInfo : ScriptableObject,
        IAsyncActorInfo<IActorModel>
    {
        private Subject<IActorModel> _valueStream = 
            new Subject<IActorModel>();
		
        public async Task<IActorModel> Create()
        {
            var model = await CreateDataSource();
            _valueStream.OnNext(model);
            return model;
        }

        public IDisposable Subscribe(IObserver<IActorModel> observer)
        {
            return _valueStream.Subscribe(observer);
        }
        
        protected abstract Task<IActorModel> CreateDataSource();
    }
}
