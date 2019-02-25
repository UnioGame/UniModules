using System;
using System.Collections.Generic;

namespace XNode
{
    public interface IUniNode
    {
        ulong Id { get; }

        /// <summary> Iterate over all outputs on this node. </summary>
        IEnumerable<NodePort> Outputs { get; }

        /// <summary> Iterate over all inputs on this node. </summary>
        IEnumerable<NodePort> Inputs { get; }

        /// <summary> Convenience function. </summary>
        /// <seealso cref="Node.AddInstancePort"/>
        /// <seealso cref="Node.AddInstanceOutput"/>
        NodePort AddInstanceInput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, string fieldName = null);

        /// <summary> Convenience function. </summary>
        /// <seealso cref="Node.AddInstancePort"/>
        /// <seealso cref="Node.AddInstanceInput"/>
        NodePort AddInstanceOutput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, string fieldName = null);

        /// <summary> Remove an instance port from the node </summary>
        void RemoveInstancePort(string fieldName);

        /// <summary> Remove an instance port from the node </summary>
        void RemoveInstancePort(NodePort port);

        /// <summary> Removes all instance ports from the node </summary>
        void ClearInstancePorts();

        /// <summary> Returns output port which matches fieldName </summary>
        NodePort GetOutputPort(string fieldName);

        /// <summary> Returns input port which matches fieldName </summary>
        NodePort GetInputPort(string fieldName);

        /// <summary> Returns port which matches fieldName </summary>
        NodePort GetPort(string fieldName);

        bool HasPort(string fieldName);

        /// <summary> Disconnect everything from this node </summary>
        void ClearConnections();
    }
}