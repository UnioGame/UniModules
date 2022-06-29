namespace UniGame.Utils.Runtime
{
    using System;
    using DG.Tweening;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private float _followDuration;
        [SerializeField] private Ease _followEase;

        private Transform _target;
        private Sequence _followSequence;
        private IDisposable _followSubscription;

        private void Awake()
        {
            this.GetLifeTime().AddCleanUpAction(() =>
            {
                _followSequence.Kill();
                _followSequence = null;
            });
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            _followSubscription?.Dispose();
            
            _followSubscription = target.ObserveEveryValueChanged(t => t.position)
                .Throttle(TimeSpan.FromMilliseconds(50))
                .Subscribe(x => BeginFollow(x))
                .AddTo(this.GetLifeTime());
        }

        private void BeginFollow(Vector3 position)
        {
            _followSequence?.Kill();
            
            _followSequence = DOTween.Sequence();

            var tween = transform.DOMove(position, _followDuration)
                .SetEase(_followEase);
            
            _followSequence.Append(tween);
            _followSequence.Play();
        }
    }
}