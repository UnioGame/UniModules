using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniNodeSystemEditor;
using UniStateMachine.Nodes;

namespace UniStateMachine.CommonNodes
{
    [CustomNodeEditorAttribute(typeof(GraphNode))]
    public class UniGraphNodeEditor : UniNodeEditor
    {
        public override void OnBodyGUI()
        {
            
            var graphNode = target as GraphNode;
            var graph = graphNode.Graph.Load<UniGraph>();
            
            base.OnBodyGUI();

            if (!graph)
                return;
            
            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });

        }
    }
    
}
