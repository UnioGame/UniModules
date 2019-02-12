using Boo.Lang;
using UniStateMachine.Nodes;
using UnityEditor.Graphs;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    public class UniGraphEditor : UnityEditor.Graphs.Graph
    {

        #region factory methods
        
        public static UniGraphEditor Create(UniNodesGraph uniGraph)
        {
            var graph = CreateInstance<UniGraphEditor>();
            graph.Initialize(uniGraph);
            return graph;
        }
        
        #endregion

        private UniGraphGuiEditor _graphGui;
        private UniNodesGraph _uniGraph;
        private List<Node> _uniNodes = new List<Node>();
        
        #region public methods

        public void Initialize(UniNodesGraph uniGraph)
        {
            _uniGraph = uniGraph;
            CreateNodes(_uniGraph);
        }
        

        public GraphGUI GetEditor()
        {
            if (_graphGui != null)
                return _graphGui;
            
            _graphGui = CreateInstance<UniGraphGuiEditor>();
            _graphGui.Initialize(_uniGraph,this);
            _graphGui.hideFlags = HideFlags.HideAndDontSave;
            return _graphGui;
            
        }
        
        #endregion

        private void CreateNodes(UniNodesGraph uniGraph)
        {
            foreach (var graphNode in uniGraph.nodes)
            {
                var editorNode = UniUnityGraphNodeEditor.Create(graphNode);
                _uniNodes.Add(editorNode);
                AddNode(editorNode);
            }
        }

    }
}
