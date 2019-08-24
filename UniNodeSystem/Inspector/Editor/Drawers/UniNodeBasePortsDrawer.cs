namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using System.Collections.Generic;
    using BaseEditor.Interfaces;
    using Runtime;
    using Runtime.Runtime;
    using Styles;

    public class UniNodeBasePortsDrawer : INodeEditorDrawer
    {
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

            var outputPort = node.GetPort(inputPortName);
            var inputPort = node.GetPort(outputPortName);

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
                var portName = portValue.ItemName;
                
                var inputPortName = node.GetFormatedInputName(portName);

                if (cache.ContainsKey(portName))
                    continue;

                var result = DrawPortPair(node, inputPortName, portName);
                
                var portOutput = node.GetPort(portName);
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
