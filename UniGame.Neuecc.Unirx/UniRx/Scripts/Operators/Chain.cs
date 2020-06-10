namespace UniRx.Operators
{
    using System;
    using System.Collections.Generic;

    internal class ChainObservable<TSource> : OperatorObservableBase<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Queue<IObservable<TSource>> _queue = new Queue<IObservable<TSource>>();

        public ChainObservable(IObservable<TSource> source) : base(true)
        {
            _source = source;
        }

        public void Skip()
        {
            // TODO: ???
        }

        public IObservable<TSource> Add(IObservable<TSource> observable)
        {
            _queue.Enqueue(observable);
            return this;
        }

        private IObservable<TSource> GetNext()
        {
            if (_queue.Count <= 0) {
                return null;
            }

            return _queue.Dequeue();
        }

        protected override IDisposable SubscribeCore(IObserver<TSource> observer, IDisposable cancel)
        {
            return new Chain(this, observer, cancel).Run();
        }

        private class Chain : OperatorObserverBase<TSource, TSource>
        {
            private readonly ChainObservable<TSource> _parent;
            private readonly SerialDisposable _serialDisposable = new SerialDisposable();

            private ChainElement _currentChainElement;

            public Chain(ChainObservable<TSource> parent, IObserver<TSource> observer, IDisposable cancel) : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var disposable = new SingleAssignmentDisposable();
                _serialDisposable.Disposable = disposable;

                _currentChainElement = new ChainElement(this, _serialDisposable);
                
                disposable.Disposable = _parent._source.Subscribe(_currentChainElement);
                return _serialDisposable;
            }

            public override void OnNext(TSource value)
            {
                try {
                    var active = _parent.GetNext();
                    
                    if (active == null) {
                        OnCompleted();
                        return;
                    }

                    _currentChainElement = new ChainElement(this, _serialDisposable);
                    _serialDisposable.Disposable = active.Subscribe(_currentChainElement);
                }
                catch (Exception ex) {
                    try {
                        observer.OnError(ex);
                    }
                    finally {
                        Dispose();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                try {
                    observer.OnError(error);
                }
                finally {
                    Dispose();
                }
            }

            public override void OnCompleted()
            {
                try {
                    observer.OnCompleted();
                }
                finally {
                    Dispose();
                }
            }
        }
        
        private class ChainElement : OperatorObserverBase<TSource, TSource>
        {
            private readonly Chain _parent;

            public ChainElement(Chain parent, IDisposable cancel) : base(parent.observer, cancel)
            {
                _parent = parent;
            }

            public override void OnNext(TSource value)
            {
                // TODO: ???
            }

            public override void OnError(Exception error)
            {
                _parent.OnError(error);
            }

            public override void OnCompleted()
            {
                _parent.OnNext(default);
            }
        }
    }
}