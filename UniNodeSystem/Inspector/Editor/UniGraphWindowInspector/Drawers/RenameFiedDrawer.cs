using UnityEditor;

namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using BaseEditor.Interfaces;
    using Interfaces;
    using Runtime.Core;

    public class RenameFiedDrawer : INodeEditorHandler
    {
    
        public bool Update(INodeEditor editor, UniBaseNode node)
        {
            var nodeName  = node.GetName();
            var nameValue = EditorGUILayout.TextField("name:", nodeName);
            if (!string.Equals(nameValue, nodeName))
            {
                node.name = nameValue;
            }
        
            return true;
        }
    }
}
