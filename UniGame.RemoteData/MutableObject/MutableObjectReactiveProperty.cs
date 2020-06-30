namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System;
    using UniRx;

    public interface INotifyable
    {
        void Notify();
    }

    public class MutableObjectReactiveProperty<T> : IReactiveProperty<T>, INotifyable
    {
        private Func<T> _getter;
        private Action<T> _setter;
        private IRemoteChangesStorage _storage;

        private ReactiveCommand<T> _changeCommand;
        public void Notify()
        {
            _changeCommand.Execute(Value);
        }

        public MutableObjectReactiveProperty(Func<T> getter, Action<T> setter, IRemoteChangesStorage storage)
        {
            _getter = getter;
            _setter = setter;
            _storage = storage;
            _changeCommand = new ReactiveCommand<T>();
        }

        public T Value { get => _getter(); set => _setter(value); }

        public bool HasValue => true;

        T IReadOnlyReactiveProperty<T>.Value => Value;
        
        public void ChangeSilentForNetwork(T value) {
            _changeCommand.Execute(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_storage.IsRootLoaded())
                observer.OnNext(Value);
            return _changeCommand.Subscribe(observer);
        }
    }
}
