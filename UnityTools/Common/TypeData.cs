namespace UniModule.UnityTools.Common
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ObjectPool.Scripts;
    using UniRx;
    
    public class TypeData : ITypeDataContainer
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
