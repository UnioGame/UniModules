using UniNodeSystem;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public interface IInputNode : INode
    {
        UniPortValue Input { get; }
    }
}