using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using UniGreenModules.UniNodeSystem.Runtime.Runtime;
using UniNodeSystemEditor;
using UnityEditor;
using UnityEngine;

public class RenameFiedDrawer : INodeEditorDrawer
{
    
    public bool Draw(INodeEditor editor, UniBaseNode node)
    {
        var nodeName = node.GetName();
        var nameValue = EditorGUILayout.TextField("name:", nodeName);
        if (!string.Equals(nameValue, nodeName))
        {
            node.name = nameValue;
        }
        
        return true;
    }
}
