using UniNodeSystem;
using UniStateMachine.CommonNodes;
using UniStateMachine.Nodes;

namespace UniStateMachine.SubGraph
{
    public class GraphOuputNode : UniNode, IGraphPortNode
    {
        
        public PortIO Direction => PortIO.Output;

        public UniPortValue PortValue => Output;

    }
    
}
