using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Drawers;
using UniNodeSystem;
using UniNodeSystemEditor;
using UnityEditor;
using UnityEngine;

public class BaseBodyDrawer : INodeEditorDrawer
{
    
    public bool Draw(INodeEditor editor, UniBaseNode node)
    {
        var serializedObject = editor.SerializedObject;

        List<string> excludes = new List<string>(){"m_Script", "graph", "position", "ports"};
        
        var iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        
        EditorGUIUtility.labelWidth = 84;
        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (excludes.Contains(iterator.name)) continue;
            
            NodeEditorGUILayout.PropertyField(iterator, true);
        }

        return true;
    }
    
}
