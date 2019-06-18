namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Nodes
{
    using System.Collections.Generic;
    using BaseEditor;
    using Drawers;
    using Runtime;
    using Runtime.Runtime;
    using UniNodeSystem.Nodes;
    using UnityEngine;

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
                    CreateNode(typeof(GraphPortNode),nodeName,target.position + new Vector2(30,30));
            }));

            return drawers;
        }
    }
    
}
