using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Malee.Editor;
using UniEditorTools;
using UnityEditor;
using UnityEngine;

namespace UniStateMachine {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniStateTransition), true)]
    public class UniTransitionNodeEditor : Editor {

        private int _selectedValidator;
        private UniStateTransition _transition;
        private ReorderableList _validatorsList;
        
        public override void OnInspectorGUI() {

            _transition = target as UniStateTransition;

            if (!_transition)
                return;

            if (!_transition.Validator) {
                var validator = _transition.AddNeted<UniTransitionValidator>("Validator");
                _transition.SetValidator(validator);
            }
            
            DrawValidators(_transition.Validator);

            serializedObject.Save();
        }

        public void DrawValidators(UniTransitionValidator validator) {

            var validators = validator.Validators;
            
            for (int i = 0; i < validators.Count; i++) {
                
                DrawValidator(validators[i]);

            }

            GUILayout.Space(2);
            EditorGUILayout.Separator();
            EditorDrawerUtils.DrawHorizontalLayout(() 
                => DrawAddValidator(validators));

        }

        private void DrawAddValidator(List<UniTransitionValidator> validators) {

            var nodeTypes = UniFSMEditorModel.ValidatorsNames;
            _selectedValidator = EditorGUILayout.
                Popup(_selectedValidator,nodeTypes);
            
            if (GUILayout.Button("+", GUILayout.MaxWidth(20))) {

                var selectedValidatorType = UniFSMEditorModel.
                    Validators[_selectedValidator];

                var transition = _transition.AddNeted(selectedValidatorType, 
                    selectedValidatorType.Name) as UniTransitionValidator;
                
                validators.Add(transition);
            }
            
        }
        
        private void DrawValidator(UniTransitionValidator validator) {

            if (!validator) return;
            
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Space(2);
            
            EditorDrawerUtils.DrawVertialLayout(() => validator.ShowCustomEditor());
            
            EditorGUILayout.EndHorizontal();

        }
        
    }
}