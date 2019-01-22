using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Input;
using UnityEngine;

public static class InputExtension
{

    public static bool IsKeyInState(this KeyCode keyCode, KeyStates keyState)
    {

        var values = UniInputSystem.KeyStatesValues;
        var states = UniInputSystem.KeyStatesItems;
        
        var keyStateValue = (long)keyState;

        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];
            var state = states[i];
            
            if(!keyStateValue.IsFlagSet(value))
                continue;

            switch (state)
            {
                case KeyStates.Down:
                    if (Input.GetKeyDown(keyCode))
                        return true;
                    break;
                case KeyStates.Up:
                    if (Input.GetKeyUp(keyCode))
                        return true;
                    break;
                case KeyStates.Pressed:
                    if (Input.GetKey(keyCode))
                        return true;
                    break;
            }
        }

        return false;
    }
    
}
