using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Attributes;
using Assets.Tools.UnityTools.Input;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class KeyNode : UniNode
    {
        [SerializeField] private List<KeyCode> _keys = new List<KeyCode>();

        [EnumFlags] [SerializeField] private KeyStates _keyStates;

        protected override IEnumerator ExecuteState(IContext context)
        {
            while (IsActive(context))
            {
                var isFire = false;

                for (var i = 0; i < _keys.Count; i++)
                {
                    var key = _keys[i];
                    var result = IsKeyActive(key);
                    if (result)
                    {
                        isFire = result;
                        break;
                    }
                }

                if (isFire)
                {
                    yield return base.ExecuteState(context);
                }
                else
                {
                    Output.RemoveContext(context);
                }

                yield return null;
            }
        }

        private bool IsKeyActive(KeyCode keyCode)
        {
            return keyCode.IsKeyInState(_keyStates);
        }
    }
}