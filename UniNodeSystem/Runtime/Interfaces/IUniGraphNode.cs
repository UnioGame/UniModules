namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System.Collections.Generic;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UniStateMachine.Runtime.Interfaces;

    public interface IUniNode : 
        INode,
        IState,
        INamedItem
    {

        IReadOnlyList<IPortValue> PortValues { get; }

        IPortValue GetPortValue(NodePort port);
        
        IPortValue GetPortValue(string port);

        bool AddPortValue(IPortValue portValue);
        
    }
}