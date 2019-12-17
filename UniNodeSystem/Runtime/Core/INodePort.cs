namespace UniGreenModules.UniNodeSystem.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface INodePort
    {
        ulong Id { get; }
        int ConnectionCount { get; }

        /// <summary> Return the first non-null connection </summary>
        NodePort Connection { get; }

        PortIO direction { get; }
        ConnectionType connectionType { get; }

        /// <summary> Is this port connected to anytihng? </summary>
        bool IsConnected { get; }

        bool IsInput { get; }
        bool IsOutput { get; }
        string fieldName { get; }
        UniBaseNode node { get; set; }
        bool IsDynamic { get; }
        bool IsStatic { get; }
        Type ValueType { get; set; }

        void UpdateId();

        /// <summary> Checks all connections for invalid references, and removes them. </summary>
        void VerifyConnections();

        /// <summary> Return the output value of this node through its parent nodes GetValue override method. </summary>
        /// <returns> <see cref="UniBaseNode.GetValue(NodePort)"/> </returns>
        object GetOutputValue();

        /// <summary> Return the output value of the first connected port. Returns null if none found or invalid.</summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        object GetInputValue();

        /// <summary> Return the output values of all connected ports. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        object[] GetInputValues();

        /// <summary> Return the output value of the first connected port. Returns null if none found or invalid. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        T GetInputValue<T>();

        /// <summary> Return the output values of all connected ports. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        T[] GetInputValues<T>();

        /// <summary> Return true if port is connected and has a valid input. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        bool TryGetInputValue<T>(out T value);

        /// <summary> Return the sum of all inputs. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        float GetInputSum(float fallback);

        /// <summary> Return the sum of all inputs. </summary>
        /// <returns> <see cref="NodePort.GetOutputValue"/> </returns>
        int GetInputSum(int fallback);

        /// <summary> Connect this <see cref="NodePort"/> to another </summary>
        /// <param name="port">The <see cref="NodePort"/> to connect to</param>
        void Connect(NodePort port);

        List<NodePort> GetConnections();
        NodePort GetConnection(int i);

        /// <summary> Get index of the connection connecting this and specified ports </summary>
        int GetConnectionIndex(NodePort port);

        bool IsConnectedTo(NodePort port);

        /// <summary> Disconnect this port from another port </summary>
        void Disconnect(NodePort port);

        /// <summary> Disconnect this port from another port </summary>
        void Disconnect(int i);

        void ClearConnections();

        /// <summary> Get reroute points for a given connection. This is used for organization </summary>
        List<Vector2> GetReroutePoints(int index);

        /// <summary> Swap connections with another node </summary>
        void SwapConnections(NodePort targetPort);

        /// <summary> Copy all connections pointing to a node and add them to this one </summary>
        void AddConnections(NodePort targetPort);

        /// <summary> Move all connections pointing to this node, to another node </summary>
        void MoveConnections(NodePort targetPort);

        /// <summary> Swap connected nodes from the old list with nodes from the new list </summary>
        void Redirect(List<UniBaseNode> oldNodes, List<UniBaseNode> newNodes);
    }
}