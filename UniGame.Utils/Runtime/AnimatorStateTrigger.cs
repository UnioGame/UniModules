namespace UniGame.Utils.Runtime
{
    using System;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class AnimatorStateTrigger : StateMachineBehaviour
    {
        [FormerlySerializedAs("_stateName")]
        [SerializeField] 
        public string StateName;

        /// <summary>
        /// Parameter - animatorStateInfo.fullPathHash
        /// </summary>
        public IObservable<string> ObserveStateEnter => _observeStateEnter;

        /// <summary>
        /// Parameter - animatorStateInfo.fullPathHash
        /// </summary>
        public IObservable<string> ObserveStateExit => _observeStateExit;

        private Subject<string> _observeStateEnter = new Subject<string>();
        private Subject<string> _observeStateExit  = new Subject<string>();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _observeStateEnter.OnNext(StateName);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _observeStateExit.OnNext(StateName);
        }
    }
}