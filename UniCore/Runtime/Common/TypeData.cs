namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Interfaces.Rx;
    using ObjectPool;
    using ObjectPool.Interfaces;
    using Rx;
    using UniRx;

    public class TypeData : ITypeData
    {
        /// <summary>
        /// registered components
        /// </summary>
        private Dictionary<Type, IValueContainerStatus> _contextValues = 
            new Dictionary<Type, IValueContainerStatus>();
                
        public bool HasValue
        {
            get {
                foreach (var value in _contextValues)
                {
                    if (value.Value.HasValue)
                        return true;
                }
                return false;
            }
        }

        #region writer methods

        public bool Remove<TData>()
        {           
            var type = typeof(TData);
            return Remove(type);
        }

        public void CleanUp()
        {
            Release();
        }

        public void Dispose()
        {
            Release();
        }

        public bool Remove(Type type)
        {
            if (!_contextValues.TryGetValue(type, out var value)) return false;
            
            var removed = _contextValues.Remove(type);
            //release context value
            if(value is IDespawnable despawnable)
                despawnable.MakeDespawn();

            return removed;

        }

        public void Add<TData>(TData value)
        {
            var data = GetData<TData>();
            data.Value = value;           
        }

        #endregion

        public IObservable<TData> GetObservable<TData>()
        {
            var data = GetData<TData>();
            return data;
        }
        
        public TData Get<TData>()
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
            return _contextValues.TryGetValue(type, out var value) && 
                   value.HasValue;
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


        private IRecycleReactiveProperty<TValue> GetData<TValue>()
        {
            IRecycleReactiveProperty<TValue> data = null;
            var type = typeof(TValue);
            
            if (!_contextValues.TryGetValue(type, out var value)) {
                value = CreateContextValue<TValue>();
                _contextValues[type] = value;
            }
            
            data = value as IRecycleReactiveProperty<TValue>;
            return data;
        }

        private IRecycleReactiveProperty<TValue> CreateContextValue<TValue>()
        {
            return ClassPool.Spawn<RecycleReactiveProperty<TValue>>();
        }

    }
}
