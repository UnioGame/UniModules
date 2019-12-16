namespace UniGreenModules.UniNodeSystem.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    /// <summary> Base class for all node graphs </summary>
    [Serializable]
    public abstract class NodeGraph : UniNode, IDisposable
    {
        #region static data

        public static List<NodeGraph> ActiveGraphs { get; } = new List<NodeGraph>();

        #endregion

        [HideInInspector] [SerializeField] private ulong _uniqueId;

        /// <summary> All nodes in the graph. <para/>
        /// See: <see cref="AddNode{T}"/> </summary>
        [SerializeField]
        public List<UniBaseNode> nodes = new List<UniBaseNode>();


        public IReadOnlyList<INode> Nodes => nodes;
        
        public ulong GetId()
        {
            return ++_uniqueId;
        }

        #region graph operations
        
        /// <summary> Add a node to the graph by type </summary>
        public T AddNode<T>() where T : UniBaseNode
        {
            return AddNode(typeof(T)) as T;
        }
        
        public T AddNode<T>(string name) where T : UniBaseNode
        {
            return AddNode(name,typeof(T)) as T;
        }

        public virtual UniBaseNode AddNode(string nodeName,Type type)
        {
            var childNode = new GameObject();
            childNode.name             = type.Name;
            childNode.transform.parent = transform;

            var node = childNode.AddComponent(type) as UniBaseNode;
            if (!node) {
                DestroyImmediate(childNode);
            }

            nodes.Add(node);
            node.graph = this;
            return node;
        }

        /// <summary> Add a node to the graph by type </summary>
        public UniBaseNode AddNode(Type type)
        {
            return AddNode(type.Name, type);
        }

        /// <summary> Creates a copy of the original node in the graph </summary>
        public virtual UniBaseNode CopyNode(UniBaseNode original)
        {
            UniBaseNode node = ScriptableObject.Instantiate(original);
            node.UpdateId();
            node.ClearConnections();
            nodes.Add(node);
            node.graph = this;
            return node;
        }

        /// <summary> Safely remove a node and all its connections </summary>
        /// <param name="node"> The node to remove </param>
        public void RemoveNode(UniBaseNode node)
        {
            node.ClearConnections();
            nodes.Remove(node);
            if (Application.isPlaying) Destroy(node);
        }

        /// <summary> Remove all nodes and connections from the graph </summary>
        public void Clear()
        {
            if (Application.isPlaying) {
                for (int i = 0; i < nodes.Count; i++) {
                    Destroy(nodes[i]);
                }
            }

            nodes.Clear();
        }

        /// <summary> Create a new deep copy of this graph </summary>
        public NodeGraph Copy()
        {
            // Instantiate a new nodegraph instance
            NodeGraph graph = Instantiate(this);
            // Instantiate all nodes inside the graph
            for (int i = 0; i < nodes.Count; i++) {
                if (nodes[i] == null) continue;
                UniBaseNode node = Instantiate(nodes[i]) as UniBaseNode;
                node.graph     = graph;
                graph.nodes[i] = node;
            }

            // Redirect all connections
            for (int i = 0; i < graph.nodes.Count; i++) {
                if (graph.nodes[i] == null) continue;
                foreach (NodePort port in graph.nodes[i].Ports) {
                    port.Redirect(nodes, graph.nodes);
                }
            }

            return graph;
        }

        public virtual void Dispose() {}
        
        #endregion
        
        private void OnDestroy()
        {
            // Remove all nodes prior to graph destruction
            Clear();
        }


    }
}