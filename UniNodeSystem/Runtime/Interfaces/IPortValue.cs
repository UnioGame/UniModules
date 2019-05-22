using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniRx;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IPortValue : 
        ITypeData, 
        ITypeValueObservable, 
        IMessageReceiver,
        IConnector<IContextWriter>
    {
    }
}