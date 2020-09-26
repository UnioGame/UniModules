namespace UniModules.UniGame.DoTweenRoutines.Runtime
{
    using Core.Runtime.DataFlow.Interfaces;
    using DG.Tweening;
    using UnityEngine;

    public static class DoTweenExtension
    {
        public static TTween AddTo<TTween>(this TTween tween, ILifeTime lifeTime)
            where  TTween : Tween
        {
            lifeTime.AddCleanUpAction(() => tween.Kill());
            return tween;
        }
        
        public static Tween MoveAnchored(this RectTransform rectTransform,Vector3 fromPosition,Vector3 toPosition,float time)
        {
            return DOTween.To(() => rectTransform.anchoredPosition,
                (Vector3 pos) => rectTransform.anchoredPosition = pos,toPosition,time).
                OnStart(() => rectTransform.anchoredPosition = fromPosition);
        }

        public static Tween MoveAnchored(this RectTransform rectTransform,Vector3 toPosition,float time)
        {
            return rectTransform.MoveAnchored(rectTransform.anchoredPosition,toPosition,time);
        }
        
        public static void KillSequence(ref Sequence sequence)
        {
            if (sequence != null) {
                sequence.Kill();
                sequence = null;
            }
        }

        public static void KillTween(ref Tween tween)
        {
            if (tween != null) {
                tween.Kill();
                tween = null;
            }
        }
    }
}