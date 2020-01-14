namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using System.Collections.Generic;
    using BaseEditor;
    using BaseEditor.Interfaces;
    using Interfaces;
    using Runtime.Core;
    using UnityEditor;

    public class BaseBodyDrawer : INodeEditorDrawer
    {
        private List<string> _excludes;
    
        public BaseBodyDrawer()
        {
            _excludes = new List<string>(){"m_Script", "graph", "position", "ports"};
        }
    
        public bool Draw(INodeEditor editor, UniBaseNode node)
        {
            var serializedObject = editor.SerializedObject;

            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;
        
            EditorGUIUtility.labelWidth = 84;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (_excludes.Contains(iterator.name)) continue;
            
                NodeEditorGUILayout.PropertyField(iterator, true);
            }

            return true;
        }
    
    }
}
