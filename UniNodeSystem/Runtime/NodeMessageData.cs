using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    public struct NodeMessageData
    {
        public string Name;
        public IPortValue Input;
        public IPortValue Output;
    }
}