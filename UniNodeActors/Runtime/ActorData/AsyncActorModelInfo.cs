namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using UniRx;
    using UnityEngine;

    public abstract class AsyncActorModelInfo<TModel>  : 
        ScriptableObject, IAsyncActorInfo<TModel>
        where TModel : IActorModel
    {

        private Subject<TModel> _valueStream = 
            new Subject<TModel>();
		
        public async Task<TModel> Create()
        {
            var model = await CreateDataSource();
            _valueStream.OnNext(model);
            return model;
        }

        public IDisposable Subscribe(IObserver<TModel> observer)
        {
            return _valueStream.Subscribe(observer);
        }
        
        protected abstract Task<TModel> CreateDataSource();

    }
}
