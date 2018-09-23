using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Assets.UI.Windows.Tools.Editor;
using LevelEditor;
using UnityEditor;
using UnityEngine;

namespace UniStateMachine {
    
    [CustomEditor(typeof(UniStateSelector), true)]
    public class UniStateSelectorEditor : Editor {

        public override void OnInspectorGUI() {
            
            var stateSelector = target as UniStateSelector;

            if (stateSelector._stateNodes == null)
                stateSelector._stateNodes = new List<UniStateTransition>();

            EditorGUILayout.BeginVertical();
            
            EditorDrawerUtils.DrawVertialLayout(() => DrawNodes(stateSelector));
            
            EditorDrawerUtils.DrawVertialLayout(() => DrawControls(stateSelector));
            
            EditorGUILayout.EndVertical();
            
            serializedObject.Save();
        }

        
        private void DrawNodes(UniStateSelector selector) {
            if (selector == null || selector._stateNodes == null)
                return;

            //draw state nodes
            var nodes = selector.Nodes;
            for (int i = 0; i < nodes.Count; i++) {

                var id = i;

                DrawNode(nodes, id);
                
                GUILayout.Space(4);
                EditorGUILayout.Space();
            }
            
            CleanUpNodes(nodes);
        }

        private void CleanUpNodes(List<UniStateTransition> nodes) {

            nodes.RemoveAll(x => x == null);

        }

        private void DrawNode(List<UniStateTransition> nodes, int index) {

            var node = nodes[index];
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
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