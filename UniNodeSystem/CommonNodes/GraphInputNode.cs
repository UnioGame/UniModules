using UniNodeSystem;
using UniStateMachine.CommonNodes;
using UniStateMachine.Nodes;

namespace UniStateMachine.SubGraph
{
    public class GraphInputNode : UniNode, IGraphPortNode
    {
        public PortIO Direction => PortIO.Input;

        public UniPortValue PortValue => Input;

        public override string GetName()
        {
            return GetFormatedInputName(base.GetName());
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();
            PortValue.Connect(Output);
        }
    }
    
}
