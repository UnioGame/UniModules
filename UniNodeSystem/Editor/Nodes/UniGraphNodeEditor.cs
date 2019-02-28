using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Nodes;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniStateMachine.Nodes;
using UnityEngine;
using XNodeEditor;

namespace UniStateMachine.CommonNodes
{
    [NodeEditor.CustomNodeEditorAttribute(typeof(GraphNode))]
    public class UniGraphNodeEditor : UniAssetNodeEditor<UniNodesGraph>
    {
        public override void OnBodyGUI()
        {
            var graphNode = target as GraphNode;
            var graph = graphNode.Target;
            
            base.OnBodyGUI();

            EditorDrawerUtils.DrawButton("show graph", 
                () => { NodeEditorWindow.Open(graph); });

        }
    }
    
}
