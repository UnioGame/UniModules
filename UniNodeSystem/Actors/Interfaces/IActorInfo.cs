using System;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel
{
    public interface IActorInfo<TModel> : 
        IFactory<TModel>, 
        IObservable<TModel> 
        where TModel : IActorModel
    {

    }
}