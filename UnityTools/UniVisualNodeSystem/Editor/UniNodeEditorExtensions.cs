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
