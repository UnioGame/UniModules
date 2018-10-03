using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using Assets.UI.Windows.Tools.Editor;
using UniEditorTools;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniStateMachine {
    [CustomEditor(typeof(UniStateMachineObject), true)]
    public class UniStateMachineEditor : Editor {
        
        public const string StateSelectorProperty = "_stateSelector";
        
        private Vector2 _nodesScroll = Vector2.zero;
        private ReorderableList _nodesList;
        private UniStateMachineObject _stateMachine;
        private SerializedProperty _selectorProperty;
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();

            _stateMachine = target as UniStateMachineObject;
            _selectorProperty = serializedObject.FindProperty(StateSelectorProperty);
            var nodeSelector = _selectorProperty.objectReferenceValue as UniStateSelector;

            if (!nodeSelector) {
                if (CreateStateSelector(_stateMachine) == false) {
                    return;
                }
            }

            if (nodeSelector == null)
                return;

            _nodesScroll = EditorDrawerUtils.
                DrawScroll(_nodesScroll,() => DrawStates(nodeSelector));
            
            serializedObject.Save();
            
        }

        private void DrawStates(UniStateSelector selector) {
            
            if (selector == null || selector._stateNodes == null)
                return;
            
            EditorGUILayout.BeginVertical();

            selector.ShowCustomEditor();

            EditorGUILayout.EndVertical();
        }

        private bool CreateStateSelector(UniStateMachineObject stateMachine) {
            
            var result = AssetEditorTools.
                SaveAssetAsNested<UniStateSelector>(stateMachine,"StateSelector");
            if (result) {
                stateMachine.SetSelector(result);
            }

            return result;
        }

    }
}