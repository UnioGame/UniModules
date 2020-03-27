namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Interfaces;
    using Interfaces.Rx;
    using ObjectPool.Runtime;
    using ObjectPool.Runtime.Extensions;
    using ObjectPool.Runtime.Interfaces;
    using UniGame.Core.Runtime.Rx;

    [Serializable]
    public class TypeData : ITypeData
    {
        private IValueContainerStatus cachedValue;
        private Type cachedType;
        
        /// <summary>
        /// registered components
        /// </summary>
        private Dictionary<Type, IValueContainerStatus> contextValues = 
            new Dictionary<Type, IValueContainerStatus>(32);

        public bool HasValue => contextValues.Any(value => value.Value.HasValue);

        #region writer methods

        public bool Remove<TData>()
        {           
            var type = typeof(TData);
            return Remove(type);
        }

        public void Dispose()
        {
            Release();
        }

        public bool Remove(Type type)
        {
            if (!contextValues.TryGetValue(type, out var value)) return false;
            
            var removed = contextValues.Remove(type);
            //release context value
            if(value is IPoolable poolable)
                poolable.Despawn();

            if (cachedType == type) {
                ResetCache();
            }
            
            return removed;

        }

        public void Publish<TData>(TData value)
        {
            var data = GetData<TData>();
            data.Value = value;           
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<TData> Receive<TData>()
        {
            var data = GetData<TData>();
            return data;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        private IRecycleReactiveProperty<TValue> CreateContextValue<TValue>() => ClassPool.Spawn<RecycleReactiveProperty<TValue>>();
        
        public bool Contains(Type type) => contextValues.TryGetValue(type, out var value) && 
                                           value.HasValue;

        public void Release()
        {
            ResetCache();
            
            foreach (var contextValue in contextValues)
            {
                if(contextValue.Value is IDisposable disposable)
                    disposable.Dispose();
            }
            
            contextValues.Clear();
        }

        private void ResetCache()
        {
            cachedType  = null;
            cachedValue = null;
        }

        private IRecycleReactiveProperty<TValue> GetData<TValue>()
        {
            if (cachedValue is IRecycleReactiveProperty<TValue> data) {
                return data;
            }
            
            var type = typeof(TValue);
            
            if (!contextValues.TryGetValue(type, out var value)) {
                value = CreateContextValue<TValue>();
                contextValues[type] = value;
            }
            
            data = value as IRecycleReactiveProperty<TValue>;
            cachedType = type;
            cachedValue = data;
            
            return data;
        }

        //Editor Only API
#if UNITY_EDITOR

        public IReadOnlyDictionary<Type, IValueContainerStatus> EditorValues => contextValues;

#endif

    }
}
