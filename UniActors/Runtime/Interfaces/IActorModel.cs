namespace UniModules.UniActors.Runtime.Interfaces
{
    using global::UniGame.Context.Runtime;
    using global::UniGame.Core.Runtime.ObjectPool;

    public interface IActorModel : 
        IContextDataSource,
        IDespawnable
    {
    }
}