using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;

namespace UniStateMachine.Nodes
{
    public interface IPortValue : 
        ITypeData, 
        ITypeValueObservable, 
        IConnector<IContextWriter>
    {
        void ConnectToPort(string portName);
    }
}