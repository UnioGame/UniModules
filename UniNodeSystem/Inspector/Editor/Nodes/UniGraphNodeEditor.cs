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
            
            var graphNode = target as GraphNode;
            var reference = graphNode.graphReference;
            if (!reference.RuntimeKeyIsValid())
                return;
            var graphObject = reference.editorAsset as GameObject;

            if (graphObject == null) return;
            
            var graph = graphObject.GetComponent<UniGraph>();
            
            base.OnBodyGUI();

            if (graph == null)
                return;
            
            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });

        }
    }
    
}
