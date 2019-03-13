using System;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace UniModule.UnityTools.Common
{
    public class TypeData : ITypeDataContainer
    {
        /// <summary>
        /// registered components
        /// </summary>
        private Dictionary<Type, object> _contextValues = new Dictionary<Type, object>();

        private ITypeDataContainer _typeDataContainerImplementation;

        public virtual TData Get<TData>()
        {
            
            var data = GetData<TData>();
            return data == null ? default(TData) : data.Value;

        }

        public bool Remove<TData>()
        {
            
            var type = typeof(TData);
            return Remove(type);

        }

        public bool Remove(Type type)
        {
            if (!_contextValues.TryGetValue(type, out var value)) return false;
            
            var typeValue = (IDisposable) value;
            var removed = _contextValues.Remove(type);

            typeValue.Dispose();
            return removed;

        }

        public void Add<TData>(TData value)
        {

            var data = GetData<TData>(true);
            data.SetValue(value);           
         
        }

        public bool Contains<TData>()
        {
            var type = typeof(TData);
            return _contextValues.ContainsKey(type);
        }

        public bool Contains(Type type)
        {
            return _contextValues.ContainsKey(type);
        }

        public IDisposable Subscribe<TData>(IObserver<TData> observer)
        {
            var data = GetData<TData>(true);
            return data.Subscribe(observer);
        }
        
        public void Release()
        {

            foreach (var contextValue in _contextValues)
            {
                var disposable = contextValue.Value as IDisposable;
                disposable?.Dispose();
            }
            
            _contextValues.Clear();

        }


        protected ContextValue<TValue> GetData<TValue>(bool isCreateIfEmpty = false)
        {
            
            ContextValue<TValue> data = null;
            
            var type = typeof(TValue);
            
            if (!_contextValues.TryGetValue(type, out var value) && isCreateIfEmpty)
            {
                data = ClassPool.Spawn<ContextValue<TValue>>();
                _contextValues[type] = data;
            }
            else
            {
                data = value as ContextValue<TValue>;
            }

            return data;

        }
        
    }
}
