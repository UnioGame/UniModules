using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using UniNodeSystem;
using UniNodeSystemEditor;
using UniStateMachine;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Drawers
{
    public class UniNodeBasePortsDrawer : INodeEditorDrawer
    {
    
        private Dictionary<string, NodePort> _drawedPorts = new Dictionary<string, NodePort>();

        public bool Draw(INodeEditor editor, UniBaseNode baseNode)
        {
        
            _drawedPorts.Clear();

            var node = baseNode as UniGraphNode;
            if (node == null)
                return true;
        
            var inputPort = node.GetPort(UniNode.InputPortName);
            var outputPort = node.GetPort(UniNode.OutputPortName);
        
            DrawPortPair(inputPort, outputPort);

            foreach (var portValue in node.PortValues)
            {
                var portName = portValue.Name;
                var formatedName = node.GetFormatedInputName(portName);

                DrawPortPair(node, portName, formatedName, _drawedPorts);
            }

            foreach (var portValue in node.PortValues)
            {
                var portName = portValue.Name;
                if (_drawedPorts.ContainsKey(portName))
                    continue;

                var port = node.GetPort(portValue.Name);
                var portStyle = GetPortStyle(port);

                port.DrawPortField(portStyle);
            }

            return true;
        }
    
    
        public void DrawPortPair(UniGraphNode node, 
            string inputPortName, string outputPortName,
            Dictionary<string, NodePort> ports)
        {
            if (_drawedPorts.ContainsKey(inputPortName))
                return;

            var outputPort = node.GetPort(inputPortName);
            var inputPort = node.GetPort(outputPortName);

            DrawPortPair(inputPort, outputPort);
        }

        public void DrawPortPair(NodePort inputPort, NodePort outputPort)
        {
            if (outputPort == null || inputPort == null)
            {
                return;
            }

            var inputStyle = GetPortStyle(inputPort);
            var outputStyle = GetPortStyle(outputPort);

            _drawedPorts[inputPort.fieldName] = inputPort;
            _drawedPorts[outputPort.fieldName] = outputPort;

            inputPort.DrawPortField(outputPort, inputStyle, outputStyle);
        }

            
        public virtual NodeGuiLayoutStyle GetPortStyle(NodePort port)
        {
            var portStyle = NodeEditorGUILayout.GetDefaultPortStyle(port);

            if (port == null)
                return portStyle;

            var uniNode = port.node as UniGraphNode;
            var portValue = uniNode.GetPortValue(port.fieldName);
            var hasData = portValue != null && portValue.Count > 0;

            if (port.fieldName == UniNode.OutputPortName || port.fieldName == UniNode.InputPortName)
            {
                portStyle.Background = Color.blue;
                portStyle.Color = hasData ? Color.red : Color.white;
                return portStyle;
            }

            if (port.IsDynamic)
            {
                portStyle.Name = port.fieldName;
                portStyle.Background = Color.red;
                portStyle.Color = port.direction == PortIO.Input ? hasData ? new Color(128, 128, 0) : Color.green :
                    hasData ? new Color(128, 128, 0) : Color.blue;
            }

            return portStyle;
        }

    }
}
