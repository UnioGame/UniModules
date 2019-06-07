using System;
using UniGreenModules.UniCore.Runtime.Interfaces;

namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    public interface IRxActorInfo<TModel>: 
        IRxFactory<TModel>, 
        IObservable<TModel> 
        where TModel : IActorModel
    {

    }
}
