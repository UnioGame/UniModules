namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.Interfaces;
    using UniStateMachine.Runtime.Interfaces;
    using UniTools.UniRoutine.Runtime;

    public interface IUniNode : 
        INode,
        IValidator<IContext>, 
        IContextState<IEnumerator>,
        INamedItem
    {

        RoutineType RoutineType { get; }
        
        IPortValue Input { get; }
        
        IPortValue Output { get; }
        
        IReadOnlyList<IPortValue> PortValues { get; }

        void UpdatePortsCache();
    }
}