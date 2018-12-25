using Assets.Tools.UnityTools.Attributes;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Input;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Validators/MouseValidator", fileName = "MouseValidator")]
public class MouseValidator : UniTransitionValidator 
{

    [SerializeField]
    private byte _mouseButtonId;

    [EnumFlags]
    [SerializeField]
    private KeyStates _keyStates;
    
    protected override bool ValidateNode(IContext context) {

        var keyValue = (long) _keyStates;
        var statesValues = UniInputSystem.KeyStatesValues;
        var states = UniInputSystem.KeyStatesItems;

        for (var i = 0; i < statesValues.Length; i++) {

            var keyStateValue = statesValues[i];
            if(!keyValue.IsFlagSet(keyStateValue))
                continue;
            
            var state = states[i];

            var stateResult = IsKeyStateActive(state);
            
            if (stateResult) {
                return true;
            }

        }

        return _defaultValue;
    }

    private bool IsKeyStateActive(KeyStates state) {
        
        switch (state) {
            case KeyStates.Down:
                return Input.GetMouseButtonDown(_mouseButtonId);
            case KeyStates.Up:
                return Input.GetMouseButtonUp(_mouseButtonId);
            case KeyStates.Pressed:
                return Input.GetMouseButton(_mouseButtonId);
            case KeyStates.StayUp: {
                var mouseUpState =
                    Input.GetMouseButtonDown(_mouseButtonId) ||
                    Input.GetMouseButtonUp(_mouseButtonId) ||
                    Input.GetMouseButton(_mouseButtonId);
                return !mouseUpState;
            }
        }

        return false;
    }
    
}
