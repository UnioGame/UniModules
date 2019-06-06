using System;
using UniGreenModules.UniCore.Runtime.Interfaces;

namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    public interface IAsyncActorInfo<TModel>: 
        IAsyncFactory<TModel>, 
        IObservable<TModel> 
        where TModel : IActorModel
    {

    }
}
