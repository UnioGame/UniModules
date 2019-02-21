using System;
using System.Collections.Generic;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEditor;
using UnityEngine;
using UniUiSystem;
using Object = UnityEngine.Object;

namespace UniTools.UniUiSystem
{
    [CustomNodeEditor(typeof(UniUiNode))]
    public class UniUiNodeEditor : UniNodeEditor
    {
        public static Type UniPortType = typeof(UniPortValue);

        private static List<IInteractionTrigger> _buttons = new List<IInteractionTrigger>();

        public override void OnBodyGUI()
        {           
            if (!(target is UniUiNode uiNode))
                return;

            base.OnBodyGUI();

            var isChanged = DrawUiNode(uiNode);
            if (isChanged)
            {
                UpdateUiData(uiNode,uiNode.UiView);
            }
        }

        public override void UpdateData(UniGraphNode node)
        {
            
            var uiNode = node as UniUiNode;

            if (!Validate(uiNode.UiView))
            {
                UpdateUiData(uiNode,uiNode.UiView);
            }
            
            base.UpdateData(node);
            
        }

        public bool DrawUiNode(UniUiNode node)
        {
            var oldView = node.UiView;
            var uiView = node.UiView.DrawObjectLayout("View");
            node.UiView = uiView;

            var isChanged = uiView != oldView;

            if (GUILayout.Button("UPDATE"))
            {
                isChanged = true;
            }

            return isChanged;
        }

        private void UpdateUiData(UniUiNode node,UiModule uiView)
        {
            if (!uiView)
            {
                return;
            }

            CollectUiData(uiView);

            //node.UiView = PrefabUtility.SavePrefabAsset(uiView.gameObject).GetComponent<UiModule>();

            EditorUtility.SetDirty(node.graph);
        }

        private bool Validate(UiModule view)
        {
            if (!view)
                return true;
            
            var triggers = view.Triggers.Items;

            for (int i = 0; i < triggers.Count; i++)
            {
                var trigger = triggers[i];
                if (string.IsNullOrEmpty(trigger.ItemName))
                    return false;
            }
            return true;
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
            CollectItems<InteractionTrigger>(module.gameObject, x =>
            {
                x.ApplyName(x.name);
                module.AddTrigger(x);
            });
        }

        private void CollectItems<TData>(GameObject target, Action<TData> action)
        {
            var items = new List<TData>();
            target.GetComponentsInChildren<TData>(true, items);

            foreach (var slot in items)
            {
                action(slot);
            }
        }
    }
}