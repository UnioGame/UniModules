using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using Modules.UniTools.UniNodeSystemEditor.Editor.Styles;
using UniNodeSystem;
using UniNodeSystemEditor;
using UniStateMachine;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Drawers
{
    public class UniNodeBasePortsDrawer : INodeEditorDrawer
    {
        private PortStyleSelector styleSelector = new PortStyleSelector();
        private Dictionary<string, NodePort> _drawedPorts = new Dictionary<string, NodePort>();

        public bool Draw(INodeEditor editor, UniBaseNode baseNode)
        {
        
            _drawedPorts.Clear();

            var node = baseNode as UniGraphNode;
            if (node == null)
                return true;

            DrawBasePorts(node,_drawedPorts);
            
            DrawPorts(node,_drawedPorts);

            return true;
        }
    
        
        public bool DrawPortPair(UniGraphNode node, 
            string inputPortName, string outputPortName)
        {

            var outputPort = node.GetPort(inputPortName);
            var inputPort = node.GetPort(outputPortName);

            return DrawPortPair(node,inputPort, outputPort);
            
        }

        public bool DrawPortPair(UniGraphNode node,NodePort inputPort, NodePort outputPort)
        {
            if (outputPort == null || inputPort == null)
            {
                return false;
            }

            var inputStyle = styleSelector.Select(inputPort);
            var outputStyle = styleSelector.Select(outputPort);

            node.DrawPortPairField(inputPort,outputPort, inputStyle, outputStyle);
            
            return true;
        }

        private void DrawPorts(UniGraphNode node,IDictionary<string, NodePort> cache)
        {
            for (var i = 0; i < node.PortValues.Count; i++)
            {
                var portValue = node.PortValues[i];
                var outputPortName = portValue.name;
                var inputPortName = node.GetFormatedInputName(outputPortName);

                if (cache.ContainsKey(outputPortName))
                    continue;

                var result = DrawPortPair(node, inputPortName,outputPortName);
                var portOutput = node.GetPort(outputPortName);
                cache[outputPortName] = portOutput;
                
                if (result)
                {
                    var portInput = node.GetPort(inputPortName);
                    cache[inputPortName] = portInput;
                }
                else
                {
                    DrawPort(portOutput);
                }
            }
        }

        public void DrawPort(NodePort port)
        {
            var portStyle = styleSelector.Select(port);
            port.DrawPortField(portStyle);
        }

        private void DrawBasePorts(UniGraphNode node,IDictionary<string, NodePort> cache)
        {
                    
            var inputPort = node.GetPort(UniGraphNode.InputPortName);
            var outputPort = node.GetPort(UniGraphNode.OutputPortName);

            cache[UniGraphNode.InputPortName] = inputPort;
            cache[UniGraphNode.OutputPortName] = outputPort;
            
            DrawPortPair(node, inputPort, outputPort);

        }

    }
}
