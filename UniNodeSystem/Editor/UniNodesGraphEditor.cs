using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.UnityGraph;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;
using UniNodeSystemEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UniGraph))]
public class UniNodesGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var graph = target as UniGraph;
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        
        if (GUILayout.Button("Show Graph", GUILayout.Height(26)))
        {
            NodeEditorWindow.Open(graph);
        }
        if (GUILayout.Button("Show Unity Graph", GUILayout.Height(26)))
        {
            UnityGraphWindow.Show(graph);
        }
        
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}
