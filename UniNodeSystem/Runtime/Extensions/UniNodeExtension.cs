namespace UniGreenModules.UniNodeSystem.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Extensions;
    using UniRx;

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

            
        public static (IPortValue inputValue, IPortValue outputValue) 
            CreatePortPair(this IUniNode node, string outputPortName, bool connectInOut = false)
        {
            var inputName = node.GetFormatedName(outputPortName,PortIO.Input);
            return node.CreatePortPair(inputName, outputPortName, connectInOut);
        }

        public static (IPortValue inputValue, IPortValue outputValue) 
            CreatePortPair(this IUniNode node,string inputPort, string outputPort, bool connectInOut = false)
        {
            var outputPortPair = node.UpdatePortValue(outputPort, PortIO.Output);
            var inputPortPair  = node.UpdatePortValue(inputPort, PortIO.Input);
                
            var inputValue  = inputPortPair.value;
            var outputValue = outputPortPair.value;
            
            if(connectInOut)
                inputValue.Connect(outputValue);
        
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

        
        public static (IPortValue , NodePort) UpdatePortValue<TValue>(this IUniNode node, 
            PortIO direction = PortIO.Output)
        {
            var type = typeof(TValue);
            var port = node.UpdatePort(type.Name, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(port.fieldName);
                node.AddPortValue(portValue);
            }

            return (portValue,port);
        
        }
        
        public static (IPortValue value, NodePort port) UpdatePortValue(this IUniNode node, 
            string portName, PortIO direction = PortIO.Output)
        {
        
            var port = node.UpdatePort(portName, direction);
            var portValue = node.GetPortValue(port);
        
            if (portValue == null)
            {
                portValue = new UniPortValue();
                portValue.ConnectToPort(portName);
                node.AddPortValue(portValue);
            }

            portValue.ConnectToPort(port.fieldName);
            
            return (portValue,port);
        
        }
        
        public static void RegisterPortHandler<TValue>(
            this IUniNode node,
            IPortValue portValue,
            IObserver<TValue> observer,
            bool oneShot = false)
        {
            //subscribe to port value observable
            portValue.GetObservable<TValue>().Finally(() => {
                    //if node stoped or 
                    if (!oneShot || !node.IsActive) return;
                    //resubscribe to port values
                    node.RegisterPortHandler(portValue, observer, true);
                }).
                Subscribe(observer). //subscribe to port value changes
                AddTo(node.LifeTime);     //stop all subscriptions when node deactivated
        }

        public static NodePort UpdatePort(
            this IUniNode node,
            string portName,
            PortIO direction = PortIO.Output)
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
                var portType = typeof(UniPortValue);

                nodePort = direction == PortIO.Output
                    ? node.AddInstanceOutput(portType, ConnectionType.Multiple, portName)
                    : node.AddInstanceInput(portType, ConnectionType.Multiple, portName);
            }

            return nodePort;
        }

    }
}