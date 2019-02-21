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

        public void DrawUiNode(UniUiNode node) 
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

            CollectUiData(uiView);
            
            node.UiView = PrefabUtility.SavePrefabAsset(uiView.gameObject).GetComponent<UiModule>();
            
            EditorUtility.SetDirty(node.graph);

        }

        private void CollectUiData(UiModule screen)
        {
            CollectSlots(screen);
            CollectTriggers(screen);
        }
        
        public void CollectSlots(UiModule module)
        {
            module.Slots.Release();
            CollectItems<IUiModuleSlot>(module.gameObject, module.AddSlot);
        }
    
        public void CollectTriggers(UiModule module)
        {
            module.Triggers.Release();
            CollectItems<IInteractionTrigger>(module.gameObject, module.AddTrigger);
        }

        private void CollectItems<TData>(GameObject target, Action<TData> action)
        {
            var items = new List<TData>();
            target.GetComponentsInChildren<TData>(true,items);

            foreach (var slot in items)
            {
                action(slot);
            }
        }
    }
}