namespace UniGreenModules.UniNodeSystem.Examples.SubGraphs
{
    using System.Collections;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class InputTestNode : UniNode
    {
        [SerializeField]
        private bool _isMouseDown = false;

        protected override IEnumerator OnExecuteState(IContext context)
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
