namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IPortValue : 
        ITypeData,
        IValueContainerStatus,
        IConnector<IContextWriter>,
        INamedItem
    {

        void ConnectToPort(string portName);

    }
}