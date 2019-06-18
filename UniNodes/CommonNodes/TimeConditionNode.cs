namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Math;
    using UnityEngine;

    public class TimeConditionNode : ConditionNode
    {
        [SerializeField]
        private float _timeInterval;

        [SerializeField]
        [EnumFlags]
        private CompareTypes CompareType;

        [SerializeField]
        private float _activeTime;

        protected override IEnumerator OnExecuteState(IContext context)
        {
            var realTime = Time.realtimeSinceStartup;
            
            nodeContext.Add(realTime);
            
            yield return base.OnExecuteState(context);
        }

        protected override bool MakeDecision(IContext context)
        {
            var realTime = Time.realtimeSinceStartup;

            var timePassed = nodeContext.Get<float>();
            timePassed = realTime - timePassed;

            var result = ValueComparator.Compare(timePassed, _timeInterval, CompareType);

            _activeTime = timePassed;
            return result;
        }
    }
}