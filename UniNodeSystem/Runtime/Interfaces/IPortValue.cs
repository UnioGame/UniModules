namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IPortValue : 
        IValueReceiver,
        ITypeValueObservable, 
        IConnector<IContextWriter>,
        INamedItem
    {
        
        
        
    }
}