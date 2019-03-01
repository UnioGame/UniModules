using System.Collections.Generic;
using UniStateMachine;
using UnityEngine;
using UniNodeSystem;
using UniNodeSystemEditor;

namespace UniStateMachine.NodeEditor
{
    public interface IUniNodeEditor : INodeEditor
    {
        bool IsSelected();

        void DrawPortPair(UniGraphNode node, string inputPortName, string outputPortName,
            Dictionary<string, NodePort> ports);

        void DrawPortPair(NodePort inputPort, NodePort outputPort, Dictionary<string, NodePort> ports);

        NodeGuiLayoutStyle GetPortStyle(NodePort port);

    }
}