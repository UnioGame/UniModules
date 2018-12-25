using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
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
            while (IsActive(context))
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
