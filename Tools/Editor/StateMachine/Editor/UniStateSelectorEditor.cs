using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Assets.UI.Windows.Tools.Editor;
using UniEditorTools;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniStateMachine {
    
    [CustomEditor(typeof(UniStateSelector), true)]
    public class UniStateSelectorEditor : Editor {

        private ReorderableList _reorderableStates;
        private UniStateSelector _stateSelector;
        
        private void OnEnable() {
            
            _stateSelector = target as UniStateSelector;
            
        }

        public override void OnInspectorGUI() {
            
            serializedObject.Update();
            
            if (_stateSelector._stateNodes == null)
                _stateSelector._stateNodes = new List<UniStateTransition>();

            EditorGUILayout.BeginVertical();
            
            EditorDrawerUtils.DrawVertialLayout(() => DrawStates(_stateSelector));
            
            EditorDrawerUtils.DrawVertialLayout(() => DrawControls(_stateSelector));
            
            EditorGUILayout.EndVertical();

            base.OnInspectorGUI();
            
            serializedObject.Save();
        }

        
        private void DrawStates(UniStateSelector selector) {
            
            if (selector == null || selector._stateNodes == null)
                return;

//            
//            //draw state nodes
//            var nodes = selector.Nodes;
//            for (int i = 0; i < nodes.Count; i++) {
//
//                var id = i;
//
//                DrawNode(nodes, id);
//                
//                GUILayout.Space(4);
//                EditorGUILayout.Space();
//            }
            
            //CleanUpNodes(nodes);
        }

        private void DrawState(Rect rect, int id, bool isActive, bool isFocused) {

            var nodes = _stateSelector.Nodes;

            var controlRect = DrawState(nodes, id);

            _reorderableStates.elementHeight = controlRect.height;
            rect.height = controlRect.height;
            rect.width = controlRect.width;

        }
        
        private void CleanUpNodes(List<UniStateTransition> nodes) {

            nodes.RemoveAll(x => x == null);

        }

        private Rect DrawState(List<UniStateTransition> nodes, int index) {

            var node = nodes[index];
            
            var controlRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.BeginVertical();

            if (!node) {
                EditorGUILayout.LabelField("[empty node]");
            }
            else {

                node.ShowCustomEditor();

            }
            
            
            EditorGUILayout.EndVertical();

            if (node && GUILayout.Button("-", GUILayout.MaxWidth(20))) {
                
                node.DestroyNestedAsset();
                nodes[index] = null; 
                
            }
            
            EditorGUILayout.EndHorizontal();

            return controlRect;
            
        }
        
        private void DrawControls(UniStateSelector selector) {

            EditorGUILayout.BeginHorizontal();

            var index = selector.Nodes.Count;
            
            if (GUILayout.Button("add node")) {
                var node = selector.AddNeted<UniStateTransition>("Transition" + index);
                selector.Nodes.Add(node);
            }
            
            EditorGUILayout.EndHorizontal();

        }
        
    }
}