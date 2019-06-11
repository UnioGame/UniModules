namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniGreenModules.UniNodeSystem.Runtime.Interfaces;

    public interface IInputNode : INode
    {
        UniPortValue Input { get; }
    }
}