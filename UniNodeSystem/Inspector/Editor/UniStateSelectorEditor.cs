using System.Collections.Generic;
using UniEditorTools;
using UnityEditor;
using UnityEngine;

namespace UniStateMachine
{
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Extensions;
    using UniGreenModules.UniNodeSystem.Runtime;

    [CustomEditor(typeof(UniStateSelector), true)]
    public class UniStateSelectorEditor : Editor
    {
        private const string NodeProperty = "_stateNodes";
        private UniStateSelector _stateSelector;
        private List<EditorValueItem<UniStateTransition>> _states;
        private bool _isDrawDefaultInspector;
        
        private void OnEnable()
        {
            if (_states != null)
            {
                _states.DespawnItems();
            }
            else
            {
                _states = new List<EditorValueItem<UniStateTransition>>();
            }
            
            _stateSelector = target as UniStateSelector;
            if (_stateSelector == null)
                return;

            if (_stateSelector.Nodes != null)
            {
                foreach (var node in _stateSelector.Nodes)
                {
                    var item = ClassPool.Spawn<EditorValueItem<UniStateTransition>>();
                    item.Value = node;
                    item.Name = node.name;
                    _states.Add(item);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            ;

            serializedObject.Update();

            _isDrawDefaultInspector = EditorDrawerUtils.ActionDrawerSwitcher(_isDrawDefaultInspector, "draw default",
                base.OnInspectorGUI, DrawCustomInspector);

            serializedObject.Save();
        }

        private void DrawCustomInspector()
        {
            if (_stateSelector._stateNodes == null)
                _stateSelector._stateNodes = new List<UniStateTransition>();

            EditorGUILayout.BeginVertical();

            EditorDrawerUtils.DrawVertialLayout(() => DrawStates(_stateSelector));

            EditorDrawerUtils.DrawVertialLayout(() => DrawControls(_stateSelector));

            EditorGUILayout.EndVertical();
        }

        private void DrawStates(UniStateSelector selector)
        {
            if (selector == null || selector.Nodes == null)
                return;

            EditorDrawerUtils.DrawListItems(_states, DrawState);

        }

        private void DrawState( int index,EditorValueItem<UniStateTransition> node)
        {
            if (!node.Value)
                return;
            
            EditorGUILayout.BeginVertical(GUI.skin.box);

            node.Value.ShowCustomEditor();
            
            EditorGUILayout.EndVertical();
            
        }

        private void DrawControls(UniStateSelector selector)
        {
            EditorGUILayout.BeginHorizontal();

            var index = selector.Nodes.Count;

            if (GUILayout.Button("add node"))
            {
                var node = selector.AddNeted<UniStateTransition>("Transition" + index);
                selector.Nodes.Add(node);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}