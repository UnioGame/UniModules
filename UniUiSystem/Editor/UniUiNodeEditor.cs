using System;
using System.Collections.Generic;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;
using UniUiSystem;

namespace UniTools.UniUiSystem
{
    
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
            
            node.UiView = PrefabUtility.SavePrefabAsset(uiView.gameObject).GetComponent<UiScreen>();
            
            EditorUtility.SetDirty(node.graph);

        }

    }
}