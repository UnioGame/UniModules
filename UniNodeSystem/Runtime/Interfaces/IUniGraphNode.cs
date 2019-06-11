namespace UniStateMachine
{
    using System.Collections;
    using System.Collections.Generic;
    using Nodes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniModule.UnityTools.UniStateMachine.Interfaces;

    public interface IUniGraphNode : IValidator<IContext>, IContextState<IEnumerator>
    {
        UniPortValue Input { get; }
        UniPortValue Output { get; }
        IReadOnlyList<UniPortValue> PortValues { get; }
    }
}