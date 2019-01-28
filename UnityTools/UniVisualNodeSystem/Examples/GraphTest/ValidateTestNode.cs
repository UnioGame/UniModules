using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
    public class ValidateTestNode : UniNode
    {
        [SerializeField]
        private bool _validateResult = true;

        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);
            while (true)
            {
                yield return null;
            }
        }

        public override bool Validate(IContext context)
        {
            return _validateResult;
        }
    }
}
