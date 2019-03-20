using System.Collections.Generic;
using UniStateMachine.Nodes;
using UnityEditor.Graphs;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    public class UniGraphGuiEditor : GraphGUI
    {
        
        private UniGraph _uniGraph;
        private UniGraphEditor _graphEditor;
        
        #region public methods

        public void Initialize(UniGraph uniGraph,UniGraphEditor graphEditor)
        {
            _uniGraph = uniGraph;
            graph = graphEditor;
        }

        public override void OnGraphGUI()
        {
            // Show node subwindows.
            m_Host.BeginWindows();

            var nodes = graph.nodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                
                var isActive = selection.Contains(node);
                var nodeStyle = Styles.GetNodeStyle(node.style, node.color, isActive);
                
                // Show the subwindow of this node.
                node.position = GUILayout.Window(
                    node.GetInstanceID(), node.position,
                    delegate { NodeGUI(node); },
                    node.title, nodeStyle, GUILayout.Width(150)
                );
            }
            
            base.OnGraphGUI();
        }


        public override void NodeGUI(Node node)
        {
            
            base.NodeGUI(node);
        }

        #endregion

    }
}
