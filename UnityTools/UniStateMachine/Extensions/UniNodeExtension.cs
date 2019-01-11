using UniStateMachine.Nodes;
using XNode;

namespace UniStateMachine
{
    public static class UniNodeExtension
    {
        public static (UniPortValue , NodePort) UpdatePortValue<TValue>(this UniGraphNode node, 
            NodePort.IO direction = NodePort.IO.Output)
        {
            var type = typeof(TValue);
            var port = node.UpdatePort<UniPortValue>(type.Name, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(port);
                node.AddPortValue(portValue);
            }

            return (portValue,port);
        
        }
        
        public static (UniPortValue value, NodePort port) UpdatePortValue(this UniGraphNode node, 
            string portName, NodePort.IO direction = NodePort.IO.Output)
        {
        
            var port = node.UpdatePort<UniPortValue>(portName, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(port);
                node.AddPortValue(portValue);
            }

            return (portValue,port);
        
        }
        

        public static NodePort UpdatePort<TValue>(this Node node,string portName,NodePort.IO direction = NodePort.IO.Output)
        {
        
            var nodePort = node.GetPort(portName);

            if (nodePort != null && nodePort.IsDynamic)
            {      
                if (nodePort.direction != direction)
                {
                    node.RemoveInstancePort(portName);
                    nodePort = null;
                }

            }
        
            if (nodePort == null)
            {
                var portType = typeof(TValue);

                nodePort = direction == NodePort.IO.Output
                    ? node.AddInstanceOutput(portType, Node.ConnectionType.Multiple, portName)
                    : node.AddInstanceInput(portType, Node.ConnectionType.Multiple, portName);
            }

            return nodePort;
        }

    }
}