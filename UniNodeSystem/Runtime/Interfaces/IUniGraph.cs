namespace UniStateMachine.Nodes
{
    using System;
    using System.Collections;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniNodeSystem;

    public interface IUniGraph : IUniGraphNode, IDisposable
    {
        /// <summary> Add a node to the graph by type </summary>
        T AddNode<T>() where T : UniBaseNode;

        /// <summary> Add a node to the graph by type </summary>
        UniBaseNode AddNode(Type type);

        /// <summary> Creates a copy of the original node in the graph </summary>
        UniBaseNode CopyNode(UniBaseNode original);

        /// <summary> Safely remove a node and all its connections </summary>
        /// <param name="node"> The node to remove </param>
        void RemoveNode(UniBaseNode node);
    }
}