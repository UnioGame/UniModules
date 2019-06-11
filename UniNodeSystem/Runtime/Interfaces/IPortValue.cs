namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IPortValue : 
        ITypeData, 
        ITypeValueObservable, 
        IMessageReceiver,
        IConnector<IContextWriter>
    {
    }
}