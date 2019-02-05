using System.Collections.Generic;
using Malee.Editor;
using UniEditorTools;
using UnityEditor;
using UnityEngine;

namespace UniStateMachine {

    public class UniTransitionNodeEditor
    {

        private const string ValidatorProperty = "_validator";
        private const string StateProperty = "_stateBehaviour";
        
        private int _selectedValidator;
        private UniStateTransition _transition;
        private ReorderableList _validatorsList;
        
        public static void Draw(SerializedProperty property)
        {
            if (property == null)
                return;

            Draw(property.objectReferenceValue as UniStateTransition);
        }

        public static void Draw(UniStateTransition transition)
        {
            if (transition == null)
                return;

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