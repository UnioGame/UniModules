namespace UniGreenModules.UniNodeSystem.Inspector.Editor.UnityGraph
{
    using System.Collections.Generic;
    using Runtime;
    using UniNodeSystem.Nodes;
    using UnityEditor.Graphs;
    using UnityEngine;

    public class UniGraphEditor : Graph
    {

        #region factory methods
        
        public static UniGraphEditor Create(UniGraph uniGraph)
        {
            var graph = CreateInstance<UniGraphEditor>();
            graph.Initialize(uniGraph);
            return graph;
        }
        
        #endregion

        private UniGraphGuiEditor _graphGui;
        private UniGraph _uniGraph;
        private List<Node> _uniNodes = new List<Node>();
        
        #region public methods

        public void Initialize(UniGraph uniGraph)
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
            _graphGui.hideFlags = HideFlags.None;
            return _graphGui;
            
        }
        
        #endregion

        private void CreateNodes(UniGraph uniGraph)
        {
            foreach (var graphNode in uniGraph.nodes)
            {
                var editorNode = UnityGraphUniNode.Create(graphNode);
                _uniNodes.Add(editorNode);
                AddNode(editorNode);
            }
        }

    }
}
