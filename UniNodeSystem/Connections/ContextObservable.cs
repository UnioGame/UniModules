using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataStructure;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class ContextObservable<TContext> : 
        ITypeDataContainer,
        IContextObservable<TContext>, 
        IPoolable
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

        public void Add<TData>(TData value)
        {
            foreach (var item in _onChangedCollection.GetItems())
            {
                //item(context);
            }
        }

        public void Release()
        {
            _onChangedCollection.Release();
            _onRemovedCollection.Release();
        }

        public bool Remove<TData>()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Type type)
        {
            return true;
        }

        public TData Get<TData>()
        {
            throw new NotImplementedException();
        }

        public bool Contains<TData>()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
