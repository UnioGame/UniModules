namespace UniGame.Utils.Runtime
{
    using Spine;
    using Spine.Unity;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Animation = Spine.Animation;

    public class SkeletonAnimationController : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("SkeletonGraphic")]
        private SkeletonGraphic _skeletonGraphic;
        [SerializeField, FormerlySerializedAs("AnimationReference")]
        private AnimationReferenceAsset _animationReference;
        [SerializeField]
        private bool _playOnStart = true;

        private Animation _animation;

        public void Play()
        {
            if (_skeletonGraphic != null)
            {
                _skeletonGraphic.freeze = false;
            }
        }

        public void Stop()
        {
            if (_skeletonGraphic != null)
            {
                _skeletonGraphic.Skeleton.Time = 0;
                _skeletonGraphic.freeze = true;
            }
        }

        public void Pause()
        {
            if (_skeletonGraphic != null)
            {
                _skeletonGraphic.freeze = true;
            }
        }

        public void UpdateAnimation(float time)
        {
            if (!Application.isPlaying && _animation == null)
            {
                if (_animationReference != null)
                    _animation = _animationReference.Animation;
            }

            if (_skeletonGraphic == null || _animation == null)
            {
                return;
            }

            _skeletonGraphic.freeze = false;

            var skeleton = _skeletonGraphic.Skeleton;
            _animation.Apply(skeleton, 0, time, _skeletonGraphic.startingLoop, null, 1f, MixBlend.Setup, MixDirection.In);
            skeleton.UpdateWorldTransform();
            _skeletonGraphic.LateUpdate();
        }

        public float GetAnimationDuration()
        {
            var duration = 0f;
            if (_skeletonGraphic != null &&
                _skeletonGraphic.skeletonDataAsset != null &&
                _skeletonGraphic.Skeleton != null &&
                _skeletonGraphic.SkeletonData != null)
            {
                duration = _skeletonGraphic.SkeletonData.FindAnimation(_skeletonGraphic.startingAnimation).Duration;
            }

            return duration;
        }

        private void Awake()
        {
            if (_animationReference != null)
                _animation = _animationReference.Animation;

            if (_playOnStart)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }
}