using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniStateMachine.CommonNodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Math;

    public class TimeConditionNode : ConditionNode
    {
        [SerializeField]
        private float _timeInterval;

        [SerializeField]
        [EnumFlags]
        private CompareTypes CompareType;

        [SerializeField]
        private float _activeTime;

        protected override IEnumerator ExecuteState(IContext context)
        {
            var realTime = Time.realtimeSinceStartup;
            
            _context.Add(realTime);
            
            yield return base.ExecuteState(context);
        }

        protected override bool MakeDecision(IContext context)
        {
            var realTime = Time.realtimeSinceStartup;

            var timePassed = _context.Get<float>();
            timePassed = realTime - timePassed;

            var result = ValueComparator.Compare(timePassed, _timeInterval, CompareType);

            _activeTime = timePassed;
            return result;
        }
    }
}