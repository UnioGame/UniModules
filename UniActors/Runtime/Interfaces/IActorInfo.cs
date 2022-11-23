namespace UniModules.UniActors.Runtime.Interfaces
{
    using System;
    using global::UniGame.Core.Runtime;

    public interface IActorInfo<out TModel> : 
        IFactory<TModel>, 
        IObservable<TModel> 
    {
        
    }
}