using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataStructure;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class ContextObservable<TContext> : IContextDataWriter<TContext>,
        IContextObservable<TContext>, IPoolable
    {
        
        private UnorderedCollection<Action<TContext>> _onChangedCollection = 
            new UnorderedCollection<Action<TContext>>();
        
        private UnorderedCollection<Action<TContext>> _onRemovedCollection = 
            new UnorderedCollection<Action<TContext>>();
        
        public IDisposable SubscribeOnContextChanged(Action<TContext> contextCreatedAction)
        {
            var id = _onChangedCollection.Add(contextCreatedAction);
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => { _onChangedCollection.Remove(id); });
            return disposable;
        }

        
        public IDisposable SubscribeOnContextRemoved(Action<TContext> removeContextAction)
        {
            var id = _onRemovedCollection.Add(removeContextAction);
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => { _onChangedCollection.Remove(id); });
            return disposable;
        }

        public void UpdateValue<TData>(TContext context, TData value)
        {
            foreach (var item in _onChangedCollection.GetItems())
            {
                item(context);
            }
        }

        public bool RemoveContext(TContext context)
        {
            foreach (var item in _onRemovedCollection.GetItems())
            {
                item(context);
            }

            return true;
        }

        public bool Remove<TData>(TContext context)
        {
            return true;
        }

        public void Release()
        {
            _onChangedCollection.Release();
            _onRemovedCollection.Release();
        }
    }
}
