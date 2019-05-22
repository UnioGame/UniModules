namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using UniModule.UnityTools.Interfaces;

    public interface IContextDataSource
    {
        void Register(IContext context);
    }
}