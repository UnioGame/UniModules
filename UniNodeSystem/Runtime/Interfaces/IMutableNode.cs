namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using Runtime;

    public interface IMutableNode
    {
        /// <summary> Convenience function. </summary>
        /// <seealso cref="UniBaseNode.AddInstancePort"/>
        /// <seealso cref="UniBaseNode.AddInstanceOutput"/>
        NodePort AddInstanceInput(Type type, ConnectionType connectionType = ConnectionType.Multiple, string fieldName = null);

        /// <summary> Convenience function. </summary>
        /// <seealso cref="UniBaseNode.AddInstancePort"/>
        /// <seealso cref="UniBaseNode.AddInstanceInput"/>
        NodePort AddInstanceOutput(Type type, ConnectionType connectionType = ConnectionType.Multiple, string fieldName = null);

        /// <summary> Remove an instance port from the node </summary>
        void RemoveInstancePort(string fieldName);

        /// <summary> Remove an instance port from the node </summary>
        void RemoveInstancePort(NodePort port);

        /// <summary> Removes all instance ports from the node </summary>
        void ClearInstancePorts();

        /// <summary> Disconnect everything from this node </summary>
        void ClearConnections();
    }
}