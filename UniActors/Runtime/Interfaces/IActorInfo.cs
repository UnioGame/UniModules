namespace UniModules.UniActors.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface IActorInfo<out TModel> : 
        IFactory<TModel>, 
        IObservable<TModel> 
    {
        
    }
}