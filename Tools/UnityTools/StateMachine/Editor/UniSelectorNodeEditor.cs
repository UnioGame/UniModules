using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(UniSelectorNode),true)]
public class UniValidatorNodeEditor : Editor 
{
    public override void OnInspectorGUI() {
        
        EditorGUILayout.LabelField("BLABLA");
        
        base.OnInspectorGUI();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background) {
        EditorGUILayout.LabelField("BLABLA");
        r.height = 100;
        base.OnPreviewGUI(r, background);
    }
}
