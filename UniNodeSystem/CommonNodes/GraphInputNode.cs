using UniNodeSystem;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public class GraphInputNode : UniNode, IGraphPortNode
    {
        public PortIO Direction => PortIO.Input;

        public UniPortValue PortValue => Input;
        
    }
    
}
