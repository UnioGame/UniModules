using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Runtime;

    public class ValidateTestNode : UniNode
    {
        [SerializeField]
        private bool _validateResult = true;

        protected override IEnumerator OnExecuteState(IContext context)
        {
            yield return base.OnExecuteState(context);
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
