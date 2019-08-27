namespace UniGreenModules.UniNodeSystem.Runtime.Connections
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;

    public interface IBrodcastConnector<TConnection> : 
        IContextWriter, 
        IConnector<TConnection>
    {
    }
}