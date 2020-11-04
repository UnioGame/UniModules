namespace UniModules.UniActors.Runtime.Interfaces
{
    using System;
    using UniGame.Core.Runtime.Interfaces;

    public interface IActorInfo<out TModel> : 
        IFactory<TModel>, 
        IObservable<TModel> 
    {
        
    }
}