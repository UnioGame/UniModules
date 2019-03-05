using UniNodeSystem;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public class GraphOuputNode : UniNode, IGraphPortNode
    {
        public PortIO Direction => PortIO.Output;

        public UniPortValue PortValue => Output;

    }
    
}
