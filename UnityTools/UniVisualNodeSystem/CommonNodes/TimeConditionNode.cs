using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Attributes;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.Math;
using UniStateMachine.CommonNodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
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
            _context.UpdateValue(context,realTime);
            
            yield return base.ExecuteState(context);
        }

        protected override bool MakeDecision(IContext context)
        {
            var realTime = Time.realtimeSinceStartup;

            var timePassed = _context.Get<float>(context);
            timePassed = realTime - timePassed;

            var result = ValueComparator.Compare(timePassed, _timeInterval, CompareType);

            _activeTime = timePassed;
            return result;
        }
    }
}