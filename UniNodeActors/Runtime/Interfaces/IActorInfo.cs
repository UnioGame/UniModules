namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using System;
    using UniModule.UnityTools.Interfaces;

    public interface IActorInfo<out TModel> : 
        IFactory<TModel>, 
        IObservable<TModel> 
        where TModel : IActorModel
    {

    }
}