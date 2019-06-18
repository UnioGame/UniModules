namespace UniGreenModules.UniUiSystem.Editor
{
    using System;
    using System.Collections.Generic;
    using Runtime;
    using Runtime.Interfaces;
    using Runtime.Triggers;
    using Runtime.UiData;
    using UniNodeSystem.Inspector.Editor.BaseEditor;
    using UniNodeSystem.Inspector.Editor.Nodes;
    using UniNodeSystem.Runtime;
    using UniRx;
    using UnityEditor;
    using UnityEngine;

    [CustomNodeEditor(typeof(UniUiNode))]
    public class UniUiNodeEditor : UniNodeEditor
    {
        
        private UiModule _moduleView;

        public override void OnBodyGUI()
        {           
            var uiNode = target as UniUiNode;
            _moduleView = uiNode.resource.editorAsset as UiModule;

            base.OnBodyGUI();

            var isChanged = DrawUiNode(uiNode);
            if (isChanged)
            {
                UpdateUiData(uiNode,uiNode.resource.editorAsset as UiModule);
            }
            
            EditorUtility.SetDirty(uiNode.graph.gameObject);
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public override void UpdateData(UniNode node)
        {
            
            var uiNode = node as UniUiNode;
            var view = uiNode.resource.editorAsset as UiModule;
            if (!Validate(view))
            {
                UpdateUiData(uiNode,view);
            }
            
            base.UpdateData(node);
            
        }

        public bool DrawUiNode(UniUiNode node)
        {
            var oldView = _moduleView;
            var uiView = node.resource.editorAsset;

            if (uiView) {
                EditorGUILayout.ObjectField(uiView, uiView.GetType(), false);
            }
            
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

            uiView.OnValidate();

            PrefabUtility.SavePrefabAsset(uiView.gameObject);
            
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

        private void CollectUiData(UiModule module)
        {
            module.OnValidate();
        }

    }
}