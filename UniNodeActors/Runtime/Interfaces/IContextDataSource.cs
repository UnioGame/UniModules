namespace UniGreenModules.UniNodeActors.Runtime
{
    using UniModule.UnityTools.Interfaces;

    public interface IContextDataSource
    {
        void Register(IContext context);
    }
}