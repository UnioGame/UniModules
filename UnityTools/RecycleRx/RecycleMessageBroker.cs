using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace Modules.UnityToolsModule.Tools.UnityTools.RecycleRx
{
	public class RecycleMessageBrocker : IMessageBroker, IPoolable
	{
        private List<RecycleMessageBrocker> _brokers = new List<RecycleMessageBrocker>(); 
		readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

		public void Publish<T>(T message)
		{
		    if (!notifiers.TryGetValue(typeof(T), out var notifier))
			{
				return;
			}
			((ISubject<T>)notifier).OnNext(message);
		}

		public IObservable<T> Receive<T>()
		{
		    if (!notifiers.TryGetValue(typeof(T), out var notifier))
			{
				var n = new Subject<T>().Synchronize();
				notifier = n;
				notifiers.Add(typeof(T), notifier);
			}

			return ((IObservable<T>)notifier).AsObservable();
		}

		public void Release()
		{
			notifiers.Clear();
		    _brokers.Clear();

        }
	}
}