using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.Extension;
    using UniGreenModules.UniCore.Runtime.Input;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class KeyNode : UniNode
    {
        [SerializeField] private List<KeyCode> _keys = new List<KeyCode>();

        [EnumFlags] [SerializeField] private KeyStates _keyStates;

        protected override IEnumerator ExecuteState(IContext context)
        {
            while (true)
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
                    Output.CleanUp();
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