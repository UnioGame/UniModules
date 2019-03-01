using System.Collections.Generic;
using UniStateMachine.Nodes;
using UnityEditor.Graphs;

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
        
        
        #endregion

    }
}
