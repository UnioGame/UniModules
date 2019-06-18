namespace UniGreenModules.UniNodeSystem.Examples.SubGraphs
{
    using System.Collections;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

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
