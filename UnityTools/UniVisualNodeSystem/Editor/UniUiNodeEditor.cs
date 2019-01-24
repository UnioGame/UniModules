using System;
using System.Collections.Generic;
using Assets.UI.Windows.Tools.Editor;
using UniEditorTools;
using UniModule.UnityTools.UiViews;
using UniStateMachine;
using UniStateMachine.NodeEditor.UiNodes;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using XNode;
using Button = UnityEngine.UI.Button;
using Object = System.Object;

namespace SubModules.Scripts.UniStateMachine.NodeEditor {
    [CustomNodeEditor(typeof(UniUiNode))]
    public class UniUiNodeEditor : UniNodeEditor 
    {
        public static Type UniPortType = typeof(UniPortValue);
        
        private static List<IInteractionTrigger> _buttons = new List<IInteractionTrigger>();

        public override void OnBodyGUI() 
        {
            base.OnBodyGUI();

            if (!(target is UniUiNode uiNode))
                return;

            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            DrawUiNode(uiNode);
        }

        public static void DrawUiNode(UniUiNode node) 
        {
            var oldView = node.UiView;
            var uiView = node.UiView.DrawObjectLayout("View");
            node.UiView = uiView;
            
            var isChanged = uiView != oldView;

            if (GUILayout.Button("UPDATE")) 
            {
                isChanged = true;
            }

            if (!isChanged) {
                return;
            }

            if (uiView == null) 
            {
                return;
            }
            
            node.UiView = PrefabUtility.SavePrefabAsset(uiView.gameObject).GetComponent<UiViewBehaviour>();
            
            EditorUtility.SetDirty(node.graph);

        }

    }
}