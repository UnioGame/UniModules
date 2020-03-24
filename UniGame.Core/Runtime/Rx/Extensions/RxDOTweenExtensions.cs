namespace UniGreenModules.UniCore.Runtime.Rx.Extensions
{
    using System;
    using DG.Tweening;
    using UniRx;

    public static class RxDOTweenExtensions
    {
        public static IObservable<Tween> OnCompleteAsObservable(this Tween tweener)
        {
            return Observable.Create<Tween>(x => { 
                tweener.OnComplete(() => {
                    x.OnNext(tweener);
                    x.OnCompleted();
                });
                return Disposable.Create(() => tweener.Kill());
            });
        }

        public static IObservable<Sequence> PlayAsObservable(this Sequence sequence)
        {
            return Observable.Create<Sequence>(x => {
                sequence.OnComplete(() => {
                    x.OnNext(sequence);
                    x.OnCompleted();
                });

                sequence.Play();

                return Disposable.Create(() => sequence.Kill());
            });
        }
    }
}