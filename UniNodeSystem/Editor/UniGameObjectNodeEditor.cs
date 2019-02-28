using System.Collections.Generic;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniStateMachine.CommonNodes;
using UnityEditor;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor
{
    [CustomNodeEditor(typeof(UniGameObjectNode))]
    public class UniGameObjectNodeEditor : UniNodeEditor
    {
        private List<Component> _components = new List<Component>();
        private bool _isComponentsShown = false;
        
        public override void OnHeaderGUI()
        {
            var node = target as UniGameObjectNode;
            var targetAsset = node.Target;
            if (node.Target)
            {
                target.name = targetAsset.name;
            }
            
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            
            var gameObjectNode = target as UniGameObjectNode;
            var targetAsset = gameObjectNode.Target;
            
            if (targetAsset)
            {
                DrawComponents(targetAsset);
            }
            
        }

        private void DrawComponents(GameObject asset)
        {

            return;
            
            asset.GetComponents(_components);

            _isComponentsShown = EditorDrawerUtils.DrawFoldout(_isComponentsShown, "Components", () => { DrawComponents(_components); });

            _components.Clear();
            
        }

        private void DrawComponents(List<Component> components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                var editor = component.GetEditor();
                editor.OnInspectorGUI();
            }
        }
    }
}
