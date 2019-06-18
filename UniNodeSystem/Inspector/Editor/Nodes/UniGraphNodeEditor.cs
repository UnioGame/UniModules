namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Nodes
{
    using BaseEditor;
    using UniEditorTools;
    using UniNodeSystem.Nodes;
    using UnityEngine;

    [CustomNodeEditor(typeof(GraphNode))]
    public class UniGraphNodeEditor : UniNodeEditor
    {
        public override void OnBodyGUI()
        {
            
            base.OnBodyGUI();

            DrawGraphNode();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private void DrawGraphNode()
        {
            
            var graphNode = target as GraphNode;

            var graph = GetTargetGraph(graphNode);
            if (graph == null)
                return;
            
            UpdateNonPlayModeData(graphNode, graph);

            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });
            
            
        }

        private void UpdateNonPlayModeData(GraphNode graphNode, UniGraph graph)
        {
            if (Application.isPlaying)
                return;
            
            graphNode.graphName = graph.name;
        }

        private UniGraph GetTargetGraph(GraphNode graphNode)
        {
            
            if (graphNode.graphInstance != null)
                return graphNode.graphInstance;
            
            var reference = graphNode.graphReference;
            if (!reference.RuntimeKeyIsValid())
                return null;
            
            var graphObject = reference.editorAsset as GameObject;
            if (graphObject == null) return null;
            
            var graph = graphObject.GetComponent<UniGraph>();

            return graph;

        }
        
    }
    
}
