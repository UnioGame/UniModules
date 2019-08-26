namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IPortValue : 
        ITypeData,
        IValueContainerStatus,
        IConnector<IContextWriter>,
        INamedItem
    {

        IObservable<Unit> PortValueChanged { get; }

        void ConnectToPort(string portName);

    }
}