namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using UniCore.Runtime.ScriptableObjects;
    using UniRx;
    using UnityEngine;

    public abstract class AsyncActorModelInfo<TModel>  : 
        AsyncActorInfo<TModel>
        where TModel : IActorModel 
    {

        public override async Task<TModel> Create()
        {
            var task = CreateDataSource();
            valueStream.OnNext(task);
            var model = await task;
            return model;
        }

    }
}
