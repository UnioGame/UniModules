using UnityEngine;
using UnityFigmaBridge.Editor.Settings;

namespace UniGame.UnityFigma.Editor.FlowGenerator
{
    using System;
    using UiSystem.Runtime.Settings;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.UiSystem.Runtime;
    using UnityEngine.UI;
    using UnityFigmaBridge.Editor;
    using UnityFigmaBridge.Editor.FigmaApi;
    using Color = UnityEngine.Color;
    
     
#if LEO_ECS_ENABLED
    using LeoEcs.ViewSystem.Behavriour;
#endif

    [CreateAssetMenu(menuName = "UniGame/Figma/View System Figma Flow",fileName = "ViewSystem FigmaFlowGenerator")]
    public class ViewSystemFigmaFlowGenerator : UnityFigmaBridgeFlowGenerator
    {
        /// <summary>
        /// Add in prototype flow functionality for this node, if required
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeGameObject"></param>
        /// <param name="figmaImportProcessData"></param>
        public override void ApplyFunctionalityToNode(
            Node node, 
            GameObject nodeGameObject,
            FigmaImportProcessData figmaImportProcessData)
        {

            if (CheckAddButtonBehaviour(node, figmaImportProcessData))
            {
                if (nodeGameObject.GetComponent<Button>() == null)
                    AddButton(nodeGameObject);
            }

            if (!figmaImportProcessData.Settings.BuildPrototypeFlow) return;
            
            // Implement button if it has a prototype connection attached
            if (string.IsNullOrEmpty(node.transitionNodeID)) return;

            var targetNodeId = node.transitionNodeID;
            var targetNode = figmaImportProcessData.NodeLookupDictionary[targetNodeId];
            var targetView = targetNode.name;
            
#if LEO_ECS_ENABLED
            var flowButton = nodeGameObject.GetComponent<OpenViewButton>();
            flowButton ??= nodeGameObject.AddComponent<OpenViewButton>();
            flowButton.view = (ViewId)targetView;
            flowButton.layoutType = GetLayoutType(targetView);
#endif
        }

        public void AddButton(GameObject nodeGameObject)
        {
            var newButtonComponent = nodeGameObject.AddComponent<Button>();

            // Find target graphic if appropriate for showing selected state
            for (var i = 0; i < nodeGameObject.transform.childCount; i++)
            {
                var child = nodeGameObject.transform.GetChild(i);
                if (!child.name.ToLower().Contains("selected")) continue;
                        
                newButtonComponent.targetGraphic = child.GetComponent<Graphic>();
                newButtonComponent.transition = Selectable.Transition.ColorTint;
                newButtonComponent.colors = new ColorBlock
                {
                    disabledColor = new Color(0, 0, 0, 0),
                    normalColor = new Color(0, 0, 0, 0),
                    highlightedColor = Color.white,
                    pressedColor = Color.white,
                    selectedColor = Color.white,
                    colorMultiplier = 1,
                };
            }
        }
        
        public override bool CheckAddButtonBehaviour(Node node, FigmaImportProcessData figmaImportProcessData)
        {
            // Apply rules
            if (node.name.ToLower().Contains("button")) return true;
            return figmaImportProcessData.Settings.BuildPrototypeFlow && 
                   !string.IsNullOrEmpty(node.transitionNodeID);
        }

        public ViewType GetLayoutType(string viewId)
        {
            var layoutType = viewId switch
            {
                not null when IsLayout(viewId, ViewType.Window) => ViewType.Window,
                not null when IsLayout(viewId, ViewType.Screen) => ViewType.Screen,
                not null when IsLayout(viewId, ViewType.Overlay) => ViewType.Overlay,
                _ => ViewType.None
            };

            return layoutType;
        }
        
        public bool IsLayout(string viewId,ViewType layout)
        {
            return viewId.Contains(layout.ToStringFromCache(),
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
