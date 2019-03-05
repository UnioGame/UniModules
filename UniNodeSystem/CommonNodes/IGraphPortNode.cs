using UniNodeSystem;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public interface IGraphPortNode : INode
    {
        PortIO Direction { get; }
        
        UniPortValue PortValue { get; }
    }
}