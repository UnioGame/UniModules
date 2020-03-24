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
        
        #if false //DOTweenPro Extensions
    static public IObservable<DOTweenAnimation> DOPlayAsObservable(
        this DOTweenAnimation animation,
        bool rewind = false)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            if (rewind) animation.DORewind();

            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DOPlay();
            return Disposable.Empty;
        });
    }

    static public IObservable<DOTweenAnimation> DOPlayByIdAsObservable(
        this DOTweenAnimation animation,
        string id,
        bool rewind = false)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            if (rewind) animation.DORewind();

            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DOPlayById(id);
            return Disposable.Empty;
        });
    }

    static public IObservable<DOTweenAnimation> DOPlayAllByIdAsObservable(
        this DOTweenAnimation animation,
        string id,
        bool rewind = false)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            if (rewind) animation.DORewind();

            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DOPlayAllById(id);
            return Disposable.Empty;
        });
    }

    static public IObservable<DOTweenAnimation> DORestartAsObservable(
        this DOTweenAnimation animation,
        bool fromHere = false)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DORestart(fromHere);
            return Disposable.Empty;
        });
    }

    static public IObservable<DOTweenAnimation> DORestartByIdAsObservable(
        this DOTweenAnimation animation,
        string id)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DORestartById(id);
            return Disposable.Empty;
        });
    }

    static public IObservable<DOTweenAnimation> DORestartAllByIdAsObservable(
        this DOTweenAnimation animation,
        string id)
    {
        return Observable.Create<DOTweenAnimation>(o =>
        {
            animation.tween.OnComplete(() =>
            {
                o.OnNext(animation);
                o.OnCompleted();
            });
            animation.DORestartAllById(id);
            return Disposable.Empty;
        });
    }
#endif //DOTweenPro Extensions
    }
}