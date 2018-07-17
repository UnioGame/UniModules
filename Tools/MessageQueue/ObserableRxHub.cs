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
        protected readonly List<IObserver<TData>> _subscribers;
        protected readonly Dictionary<IResetable, int> _subscribersData;
        protected readonly Dictionary<IObservable<TData>, IDisposable> _observablesMap;

        protected List<IObserver<TData>> _unsubscribedObservers;

        #endregion

        #region constructor

        public ObserableRxHub() {
            _subscribers = new List<IObserver<TData>>();
            _subscribersData = new Dictionary<IResetable, int>();

            _observablesMap = new Dictionary<IObservable<TData>, IDisposable>();
            _unsubscribedObservers = new List<IObserver<TData>>();
        }

        #endregion

        public IResetable Subscribe(IObserver<TData> observer) {

            _subscribers.Add(observer);

            var messageCancellation = ClassPool.Spawn<MessageCancellation<TData>>();
            messageCancellation.Initialize(observer, _unsubscribedObservers);
            _subscribersData[messageCancellation] = _subscribers.Count;
            
            return messageCancellation;
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
            foreach (var disposable in _subscribersData) {
                disposable.Key.Cancel();
            }
            _subscribers.Clear();
            _subscribersData.Clear();
        }

        #endregion


    }
}
