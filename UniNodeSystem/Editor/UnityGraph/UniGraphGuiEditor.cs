using System.Collections.Generic;
using UniStateMachine.Nodes;
using UnityEditor.Graphs;

namespace Modules.UniTools.UniNodeSystem.Editor.UnityGraph
{
    public class UniGraphGuiEditor : GraphGUI
    {
        
        private UniNodesGraph _uniGraph;
        private UniGraphEditor _graphEditor;
        
        #region public methods

        public void Initialize(UniNodesGraph uniGraph,UniGraphEditor graphEditor)
        {
            _uniGraph = uniGraph;
            graph = graphEditor;
        }
        
        
        #endregion

    }
}
