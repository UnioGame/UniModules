namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using BaseEditor;
    using BaseEditor.Interfaces;
    using Runtime.Runtime;
    using UniEditorTools;
    using UnityEditor;
    using UnityEngine;

    public class BaseHeaderDrawer : INodeEditorDrawer
    {
        public virtual bool Draw(INodeEditor editor, UniBaseNode node)
        {
            var target = node;
            var title = target.GetName();
            var renaming = NodeEditor.Renaming;

            if (NodeEditor.Renaming != 0 && Selection.Contains(target))
            {
                int controlId = GUIUtility.GetControlID(FocusType.Keyboard) + 1;
                if (renaming == 1)
                {
                    GUIUtility.keyboardControl = controlId;
                    EditorGUIUtility.editingTextField = true;
                    NodeEditor.Renaming = 2;
                }

                target.name = EditorGUILayout.TextField(target.name, NodeEditorResources.styles.nodeHeader,
                    GUILayout.Height(30));
                
                if (!EditorGUIUtility.editingTextField)
                {
                    editor.Rename(target.name);
                    NodeEditor.Renaming = 0;
                }
            }
            else
            {
                GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
                node.width = (int)EditorGUILayout.Slider(node.width, 220, 1000);
            }

            return true;
        }
    }
}