namespace UniModules.UniActors.Runtime.ActorData
{
    using System.Threading.Tasks;
    using Interfaces;

    public abstract class AsyncActorInfo<TModel> : 
        BaseActorInfo<Task<TModel>>
        where TModel : IActorModel 
    {
    }
}
