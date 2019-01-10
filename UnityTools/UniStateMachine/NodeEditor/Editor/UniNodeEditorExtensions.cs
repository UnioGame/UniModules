using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;
using XNodeEditor;

public static class UniNodeEditorExtensions
{
    
    public static GUILayoutOption[] DefaultPortOptions = new GUILayoutOption[0];

    public static GUILayoutOption[] MainPortOptions = new GUILayoutOption[0];
    
    public static NodePort DrawPortField(this NodePort port, GUIContent label,GUILayoutOption[] options) {

        NodeEditorGUILayout.PortField(label,port,options);
        return port;

    }

    public static (UniPortValue , NodePort) UpdatePortValue(this UniGraphNode node, 
        string portName, NodePort.IO direction = NodePort.IO.Output)
    {
        
        var port = node.UpdatePort<UniPortValue>(portName, direction);
        var portValue = node.GetPortValue(port);
        
        if (portValue == null)
        {
            portValue = new UniPortValue();
            portValue.ConnectToPort(port);
            node.AddPortValue(portValue);
        }

        return (portValue,port);
        
    }

    public static NodePort UpdatePort<TValue>(this Node node,string portName,NodePort.IO direction = NodePort.IO.Output)
    {
        
        var nodePort = node.GetPort(portName);

        if (nodePort != null && nodePort.IsDynamic)
        {      
            if (nodePort.direction != direction)
            {
                node.RemoveInstancePort(portName);
                nodePort = null;
            }

        }
        
        if (nodePort == null)
        {
            var portType = typeof(TValue);

            nodePort = direction == NodePort.IO.Output
                ? node.AddInstanceOutput(portType, Node.ConnectionType.Multiple, portName)
                : node.AddInstanceInput(portType, Node.ConnectionType.Multiple, portName);
        }

        return nodePort;
    }

    public static NodePort DrawPortField(this NodePort port, NodeGuiLayoutStyle style)
    {

        NodeEditorGUILayout.PortField(port, style);
        
        return port;

    }

    public static NodePort DrawPortField(this NodePort port, GUILayoutOption[] options)
    {

        NodeEditorGUILayout.PortField(null, port, options);
        return port;

    }

}
