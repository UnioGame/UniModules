using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.UnityGraph;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(UniNodesGraph))]
public class UniNodesGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Show Graph", GUILayout.Height(26)))
            UnityGraphWindow.Show(target as UniNodesGraph);
        
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}
