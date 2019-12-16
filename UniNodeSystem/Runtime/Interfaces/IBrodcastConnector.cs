namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IBrodcastConnector<TConnection> : 
        IContextWriter, 
        IConnector<TConnection>
    {
    }
}