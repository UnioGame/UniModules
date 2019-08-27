namespace UniGreenModules.UniActors.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IContextDataSource
    {
        void Register(IContext context);
    }
}