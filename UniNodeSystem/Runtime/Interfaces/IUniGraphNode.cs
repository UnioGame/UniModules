namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.Interfaces;
    using UniModule.UnityTools.UniStateMachine.Interfaces;

    public interface IUniGraphNode : IValidator<IContext>, IContextState<IEnumerator>
    {
        UniPortValue Input { get; }
        UniPortValue Output { get; }
        IReadOnlyList<UniPortValue> PortValues { get; }
    }
}