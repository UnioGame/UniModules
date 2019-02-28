using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.Drawers;
using Modules.UniTools.UniNodeSystem.Editor.Nodes;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniStateMachine.CommonNodes;
using UnityEngine;

namespace Modules.UniTools.UniNodeSystem.Editor
{
    [CustomNodeEditor(typeof(GameObjectNode))]
    public class UniGameObjectNodeEditor : UniAssetNodeEditor<GameObject>
    {
        private List<Component> _components = new List<Component>();
        private bool _isComponentsShown = false;
        
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            
            var gameObjectNode = target as GameObjectNode;
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
