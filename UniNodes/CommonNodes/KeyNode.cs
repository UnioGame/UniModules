namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.Extension;
    using UniCore.Runtime.Input;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UnityEngine;

    public class KeyNode : UniNode
    {
        [SerializeField] private List<KeyCode> _keys = new List<KeyCode>();

        [EnumFlags] [SerializeField] private KeyStates _keyStates;

        protected override IEnumerator OnExecuteState(IContext context)
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
                    yield return base.OnExecuteState(context);
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