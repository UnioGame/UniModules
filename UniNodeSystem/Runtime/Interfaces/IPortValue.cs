namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IPortValue : 
        IValueReceiver,
        IMessageBroker,
        IConnector<IContextWriter>,
        INamedItem
    {
        
        
        
    }
}