using UniGreenModules.UniNodeSystem.Runtime.Runtime;
using UnityEditor;

namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using BaseEditor.Interfaces;

    public class RenameFiedDrawer : INodeEditorDrawer
    {
    
        public bool Draw(INodeEditor editor, UniBaseNode node)
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
