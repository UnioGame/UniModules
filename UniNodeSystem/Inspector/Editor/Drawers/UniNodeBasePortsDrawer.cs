namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using BaseEditor.Interfaces;
    using Interfaces;
    using Runtime;
    using Runtime.Core;
    using Runtime.Extensions;
    using Styles;

    public class UniNodeBasePortsDrawer : INodeEditorDrawer
    {
        private Regex bracketsExpr = new Regex(UniNodeExtension.InputPattern);
        private PortStyleSelector styleSelector = new PortStyleSelector();
        private Dictionary<string, NodePort> _drawedPorts = new Dictionary<string, NodePort>();

        public bool Draw(INodeEditor editor, UniBaseNode baseNode)
        {
        
            _drawedPorts.Clear();

            var node = baseNode as UniNode;
            if (node == null)
                return true;

            DrawPorts(node,_drawedPorts);

            return true;
        }
    
        
        public bool DrawPortPair(
            UniNode node, 
            string inputPortName, 
            string outputPortName)
        {

            var inputPort = node.GetPort(inputPortName);
            var outputPort = node.GetPort(outputPortName);

            return DrawPortPair(node,inputPort, outputPort);
            
        }

        public bool DrawPortPair(UniNode node,NodePort inputPort, NodePort outputPort)
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

        private void DrawPorts(UniNode node,IDictionary<string, NodePort> cache)
        {
            for (var i = 0; i < node.PortValues.Count; i++)
            {
                var portValue = node.PortValues[i];
                var outputPortName = portValue.ItemName;
                
                if (cache.ContainsKey(outputPortName))
                    continue;
                
                var portName = bracketsExpr.Replace(outputPortName, string.Empty, 1);
                var inputPortName = portName.GetFormatedPortName(PortIO.Input);

                var result = DrawPortPair(node, inputPortName, outputPortName);
                
                var portOutput = node.GetPort(outputPortName);
                cache[portName] = portOutput;
                
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

    }
}
