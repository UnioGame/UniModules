using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Boo.Lang;
using UniStateMachine.Nodes;
using XNode;

namespace UniStateMachine
{
    public static class UniNodeExtension
    {
        
        public static List<TTarget> GetOutputConnections<TTarget>(this Node node)
            where TTarget :Node
        {
            var items = ClassPool.Spawn<List<TTarget>>();
            
            foreach (var nodeOutput in node.Outputs)
            {
                var connectedNode = nodeOutput.GetConnectedNode<TTarget>();
                if(connectedNode == null)
                    continue;
                items.Add(connectedNode);
            }

            return items;
        }

        public static List<NodePort> GetConnectionToNodes<TTarget>(this NodePort port)
            where TTarget :Node
        {
            
            var items = ClassPool.Spawn<List<NodePort>>();
            
            var connections = port.GetConnections();
            
            foreach (var connection in connections)
            {
                if(!(connection.node is TTarget node))
                    continue;
                
                items.Add(connection);
            }
            
            connections.DespawnCollection();
            
            return items;
            
        }

        public static void CopyTo(this UniPortValue from, UniPortValue to)
        {
            foreach (var context in from.Contexts)
            {
                var writer = to.GetWriter(context);
                from.CopyTo(context,writer);
            }
        }
        
        public static List<TTarget> GetConnectedNodes<TTarget>(this NodePort port)
            where TTarget :Node
        {
            var items = ClassPool.Spawn<List<TTarget>>();
            
            var connections = port.GetConnections();
            
            foreach (var connection in connections)
            {
                if(!(connection.node is TTarget node))
                    continue;
                items.Add(node);
            }
            
            connections.DespawnCollection();
            
            return items;
        }

        public static TValue GetConnectedNode<TValue>(this NodePort port)
            where TValue :Node
        {
            if (port == null || !port.IsConnected)
            {
                return null;
            }
            if (!(port.node is TValue item))
            {
                return null;
            }

            return item;
        }

        
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