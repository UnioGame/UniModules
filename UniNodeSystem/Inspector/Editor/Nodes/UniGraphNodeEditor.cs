namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Nodes
{
    using BaseEditor;
    using Runtime;
    using UniEditorTools;

    [CustomNodeEditor(typeof(GraphNode))]
    public class UniGraphNodeEditor : UniNodeEditor
    {
        public override void OnBodyGUI()
        {
            
            var graphNode = target as GraphNode;
            var graph = graphNode.GetGraph();
            
            base.OnBodyGUI();

            if (!graph)
                return;
            
            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });

        }
    }
    
}
