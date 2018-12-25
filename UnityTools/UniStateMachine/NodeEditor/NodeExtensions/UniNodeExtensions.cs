using System.Collections.Generic;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using XNode;

namespace UniStateMachine.NodeEditor.NodeExtensions
{
    public static class UniNodeExtensions 
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
        
        
    }
}
