namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ObjectPool;

    public class TypeData : ITypeData
    {
        /// <summary>
        /// registered components
        /// </summary>
        private Dictionary<Type, IDataValueParameters> _contextValues = new Dictionary<Type, IDataValueParameters>();

        #region writer methods
        
        public bool Remove<TData>()
        {           
            var type = typeof(TData);
            return Remove(type);
        }

        public void RemoveAll()
        {
            Release();
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

        #endregion
        
        public bool HasValue()
        {
            foreach (var value in _contextValues)
            {
                if (value.Value.HasValue())
                    return true;
            }
            return false;
        }
        
        public virtual TData Get<TData>()
        {
            var data = GetData<TData>();
            return data == null ? default(TData) : data.Value;
        }

        public bool Contains<TData>()
        {
            var type = typeof(TData);
            return Contains(type);
        }

        public bool Contains(Type type)
        {
            if (_contextValues.TryGetValue(type, out var value))
            {
                return value.HasValue();
            }
            return false;
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
                if(contextValue.Value is IDisposable disposable)
                    disposable.Dispose();
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
