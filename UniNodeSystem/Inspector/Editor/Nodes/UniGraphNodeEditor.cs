namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Nodes
{
    using BaseEditor;
    using Runtime;
    using Runtime.Interfaces;
    using UniEditorTools;
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
            
            var reference = graphNode.graphReference;
            if (!reference.RuntimeKeyIsValid())
                return;
            
            var graphObject = reference.editorAsset as GameObject;
            if (graphObject == null) return;
            
            var graph = graphObject.GetComponent<UniGraph>();
            if (graph == null)
                return;

            graphNode.graphName = graph.name;
            
            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });
            
            
        }
        
    }
    
}
