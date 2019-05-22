using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class InputTestNode : UniNode
    {
        [SerializeField]
        private bool _isMouseDown = false;

        protected override IEnumerator ExecuteState(IContext context)
        {

            while (true)
            {
                yield return null;

                _isMouseDown = UnityEngine.Input.GetMouseButton(0);
                if (_isMouseDown)
                {
                    Output.Add(context);
                }
                else
                {
                    Output.Remove<IContext>();
                }

            }

        }
    }
}
