namespace UniGreenModules.UniContextData.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IContextDataSource
    {
        void Register(IContext context);
    }
}