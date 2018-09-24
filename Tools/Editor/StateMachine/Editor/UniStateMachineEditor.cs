using System;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Assets.UI.Windows.Tools.Editor;
using LevelEditor;
using Tools.ReflectionUtils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniStateMachine {
    [CustomEditor(typeof(DataContextStateMachine), true)]
    public class UniStateMachineEditor : Editor {
        
        private Vector2 _nodesScroll = Vector2.zero;
        private ReorderableList _nodesList;
        private DataContextStateMachine _stateMachine;

        public override void OnInspectorGUI() {
            
            //base.OnInspectorGUI();

            _stateMachine = target as DataContextStateMachine;
            var nodeSelector = _stateMachine.StateSelector;

            if (!nodeSelector) {
                if (CreateNodeSelector(_stateMachine) == false) {
                    return;
                }
            }

            if (nodeSelector == null)
                return;

            _nodesScroll = EditorDrawerUtils.
                DrawScroll(_nodesScroll,() => DrawNodes(nodeSelector));
            
            serializedObject.Save();
            
        }

        private void DrawNodes(DataContextStateSelector selector) {
            
            if (selector == null || selector._stateNodes == null)
                return;
            
            EditorGUILayout.BeginVertical();

            selector.ShowCustomEditor();

            EditorGUILayout.EndVertical();
        }

        private bool CreateNodeSelector(DataContextStateMachine stateMachine) {
            
            var result = AssetEditorTools.
                SaveAssetAsNested<DataContextStateSelector>(stateMachine,"StateSelector");
            if (result) {
                stateMachine.SetSelector(result);
            }

            return result;
        }

    }
}