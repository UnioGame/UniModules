using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniRx;

namespace UniStateMachine.Nodes
{
    public interface IPortValue : 
        ITypeData, 
        ITypeValueObservable, 
        IMessageReceiver,
        IConnector<IContextWriter>
    {
    }
}