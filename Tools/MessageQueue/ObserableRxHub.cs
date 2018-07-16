using System;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Assets.Tools.Utils;
using UniRx;

namespace Assets.Scripts.MessageQueue
{
    public class ObserableRxHub<TData> : IMessageSystem<TData>
    {

        #region private properties

        /// <summary>
        /// added channels
        /// </summary>
        protected readonly Dictionary<IObserver<TData>, IResetable> _subscribers;
        protected readonly Dictionary<IObservable<TData>, IDisposable> _observablesMap;

        private List<IObserver<TData>> _unsubscribedObservers;

        #endregion

        #region constructor

        public ObserableRxHub() {
            _subscribers = new Dictionary<IObserver<TData>, IResetable>();
            _observablesMap = new Dictionary<IObservable<TData>, IDisposable>();
            _unsubscribedObservers = new List<IObserver<TData>>();
        }

        #endregion

        public IResetable Subscribe(IObserver<TData> observer) {

            IResetable resetable = null;
            if (_subscribers.TryGetValue(observer, out resetable) == false) {

                var messageCancellation = ClassPool.Spawn<MessageCancellation<TData>>();
                messageCancellation.Initialize(observer, _unsubscribedObservers);
                resetable = messageCancellation;
                _subscribers[observer] = resetable;
            }

            return resetable;
        }

        public IDisposable AddPublicher(IObservable<TData> observable) {

            IDisposable disposable = null;
            if (_observablesMap.TryGetValue(observable,out disposable) == false)
            {
                disposable = observable.Subscribe(x => OnMessageReceived(x, observable));
                _observablesMap[observable] = disposable;
            }

            return disposable;
        }

        #region private methods

        protected virtual void OnMessageReceived(TData message, IObservable<TData> sender)
        {

        }


        public void Dispose()
        {
            foreach (var disposable in _subscribers) {
                disposable.Value.Cancel();
            }
            _subscribers.Clear();
        }

        #endregion


    }
}
