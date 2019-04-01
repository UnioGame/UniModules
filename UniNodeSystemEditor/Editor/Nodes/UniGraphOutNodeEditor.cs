using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniNodeSystem;
using UniNodeSystemEditor;
using UniStateMachine.SubGraph;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    [CustomNodeEditorAttribute(typeof(GraphOuputNode))]
    public class UniGraphOutputNodeEditor : UniNodeEditor
    {
        protected override List<INodeEditorDrawer> InitializedBodyDrawers()
        {
            var drawers = base.InitializedBodyDrawers();
            
            drawers.Add(new RenameFiedDrawer());
            drawers.Add(new ButtonActionBodyDrawer("add in", () =>
            {
                if(NodeEditorWindow.current == null)
                    return;
                var nodeName = target.GetName().
                    Replace(UniBaseNode.InputTriggerPrefix, "");
                NodeEditorWindow.current.
                    CreateNode(typeof(GraphInputNode),nodeName,target.position + new Vector2(30,30));
            }));

            return drawers;
        }
    }
    
}
