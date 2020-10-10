namespace UniModules.UniActors.Runtime.Interfaces
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IActorModel : 
        IContextDataSource,
        IDespawnable
    {
    }
}