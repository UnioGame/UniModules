using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UnityTools.Common;
using UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.Common
{
    public class TypeData : ITypeDataContainer, IDataWriter
    {
        /// <summary>
        /// registered components
        /// </summary>
        private Dictionary<Type, IWritableValue> _contextValues = new Dictionary<Type, IWritableValue>();

        private ITypeDataContainer _typeDataContainerImplementation;

        public IReadOnlyCollection<IWritableValue> Values => _contextValues.Values;
        
        public virtual TData Get<TData>()
        {
            
            var data = GetData<TData>();
            return data == null ? default(TData) : data.Value;

        }

        public bool Remove<TData>()
        {
            var type = typeof(TData);
            
            if (_contextValues.TryGetValue(type, out var value))
            {
                var typeValue = (ContextValue<TData>)value;
                var removed = _contextValues.Remove(type);

                typeValue.Dispose();
                return removed;
            }

            return false;
        }

        public void Add<TData>(TData value)
        {

            var data = GetData<TData>(true);
            data.SetValue(value);           
         
        }

        public bool HasData<TData>()
        {
            var type = typeof(TData);
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
