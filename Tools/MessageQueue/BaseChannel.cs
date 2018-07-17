using System;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Assets.Scripts.MessageQueue;
using UniRx;

namespace Assets.Scripts.MessageQueue {

    public class BaseChannel : IChannel {

        protected Subject<IMessage> _input;
        protected Subject<IMessage> _output;
        protected Subject<Type> _typeAttached;
        protected Subject<Type> _typeRemoved;

        private List<IDisposable> _disposables;
        private Dictionary<Type, object> _registeredTypes;

        private IResetable _inputDisposable;
        private IDisposable _outputDisposable;
        private IMessageSystem<IMessage> _messageSystem;

        public BaseChannel(IMessageSystem<IMessage> messageSystem) {

            _messageSystem = messageSystem;

            _input = new Subject<IMessage>();
            _output = new Subject<IMessage>();
            _disposables = new List<IDisposable>();
            _registeredTypes = new Dictionary<Type, object>();

            _outputDisposable = messageSystem.AddPublicher(Output);
        }

        public bool IsDisposed { get; protected set; }

        public IObserver<IMessage> Input => _input;

        public IObservable<IMessage> Output => _output;

        public IObservable<Type> OnAttach => _typeAttached;

        public IObservable<Type> OnUnsubscribe => _typeRemoved;

        public void Send(IMessage message) {

            ProcessMessage(message, _output, ValidateOutputMessage);

        }

        public bool IsRegistered(Type type) {
            return _registeredTypes.ContainsKey(type);
        }

        public IObservable<T> Attach<T>()
            where T : class {

            var targetType = typeof(T);
            if(_inputDisposable == null)
                _inputDisposable = _messageSystem.Subscribe(_input);

            if (_registeredTypes.TryGetValue(targetType, out object outObservable) == false) {

                outObservable = _input.Select(x => x.Context as T).Where(x => x != null);
                _registeredTypes.Add(targetType, outObservable);

            }

            return outObservable as IObservable<T>;
        }

        public void SendToInput(IMessage message) {

            ProcessMessage(message, _input, ValidateInputMessage);

        }

        public void Dispose() {
            _input.OnCompleted();
            _output.OnCompleted();

            _input.Cancel();
            _output.Cancel();
            _outputDisposable.Cancel();
            _outputDisposable = null;

            if (_inputDisposable != null)
                _inputDisposable.Cancel();
            _inputDisposable = null;

            _disposables.Cancel();
            _disposables.Clear();

            IsDisposed = true;
        }

        #region private methods

        protected virtual bool ValidateOutputMessage(IMessage message) {

            return true;

        }

        protected virtual bool ValidateInputMessage(IMessage data) {

            return true;

        }


        private void ProcessMessage(IMessage messageObject, IObserver<IMessage> observable,
            Func<IMessage, bool> validator) {
            if (messageObject == null) return;

            var validateResult = validator(messageObject);
            if (validateResult == false) return;
            observable.OnNext(messageObject);

        }

        #endregion
    }
}