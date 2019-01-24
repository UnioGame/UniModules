using System.Collections.Generic;
using System.Linq;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniStateMachine;
using UniStateMachine.CommonNodes;
using XNodeEditor;

namespace UniModule.UnityTools.UniNodesEditorSystem
{
    [NodeEditor.CustomNodeEditorAttribute(typeof(UniModuleNode))]
    public class ModuleNodeEditor : UniNodeEditor
    {
        public override void OnBodyGUI()
        {
            var node = target as UniModuleNode;
            var items = node.ModulePortValues.ToList();
            
            base.OnBodyGUI();

            var newItems = node.ModulePortValues;

            if (newItems.Count== items.Count && 
                items.SequenceEqual(newItems)){
                return;
            }

            UpdateModulePorts(node, items);
        }

        private void UpdateModulePorts(UniModuleNode node, List<string> ports)
        {
            foreach (var portName in ports)
            {
                var port = node.GetPort(portName);
                if(port == null) continue;
                node.RemoveInstancePort(port);
            }
            node.UpdatePortsCache();
        }
    }
}
