using System.Collections.Generic;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine.Profiling;
using UniNodeSystem;

namespace UniModule.UnityTools.UniStateMachine.Extensions
{
    public static class UniNodeExtension
    {
        
        public static List<TTarget> GetOutputConnections<TTarget>(this UniBaseNode node)
            where TTarget :UniBaseNode
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

            
        public static (UniPortValue inputValue, UniPortValue outputValue) CreatePortPair(this UniNode node, string outputPortName, bool connectInOut = false)
        {
            var inputName = node.GetFormatedInputName(outputPortName);
            var inputPortPair = node.UpdatePortValue(inputName, PortIO.Input);
            var outputPortPair = node.UpdatePortValue(outputPortName, PortIO.Output);
                
            var inputValue = inputPortPair.value;
            var outputValue = outputPortPair.value;
            
            if(connectInOut)
                inputValue.Add(outputValue);
        
            return (inputValue,outputValue);
        }

        
        public static List<NodePort> GetConnectionToNodes<TTarget>(this NodePort port)
            where TTarget :UniBaseNode
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
        
        public static List<TTarget> GetConnectedNodes<TTarget>(this NodePort port)
            where TTarget :UniBaseNode
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
            where TValue :UniBaseNode
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
            PortIO direction = PortIO.Output)
        {
            var type = typeof(TValue);
            var port = node.UpdatePort<UniPortValue>(type.Name, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(port.fieldName);
                node.AddPortValue(portValue);
            }

            return (portValue,port);
        
        }
        
        public static (UniPortValue value, NodePort port) UpdatePortValue(this UniGraphNode node, 
            string portName, PortIO direction = PortIO.Output)
        {
        
            var port = node.UpdatePort<UniPortValue>(portName, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(port.fieldName);
                node.AddPortValue(portValue);
            }

            return (portValue,port);
        
        }
        

        public static NodePort UpdatePort<TValue>(this UniBaseNode node,string portName,PortIO direction = PortIO.Output)
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

                nodePort = direction == PortIO.Output
                    ? node.AddInstanceOutput(portType, ConnectionType.Multiple, portName)
                    : node.AddInstanceInput(portType, ConnectionType.Multiple, portName);
            }

            return nodePort;
        }

    }
}