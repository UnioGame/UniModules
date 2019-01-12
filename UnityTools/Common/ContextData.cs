using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UnityTools.Common;
using UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.Common
{
    public class ContextData : IContextData, IDataTransition
    {
        /// <summary>
        /// registered conmponents
        /// </summary>
        private Dictionary<Type, IDataCopier<IDataTransition>> _contextValues = new Dictionary<Type, IDataCopier<IDataTransition>>();

        public virtual TData Get<TData>()
        {
            
            if (!_contextValues.TryGetValue(typeof(TData), out var value))
            {
                return default(TData);
            }

            var valueData = value as ContextValue<TData>;
            return valueData.Value;

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

        public void Add<TData>(TData data)
        {
            
            ContextValue<TData> contextValue = null;
            var type = typeof(TData);

            //value already exists, replace it
            if (_contextValues.TryGetValue(type, out IDataCopier<IDataTransition> value))
            {
                contextValue = (ContextValue<TData>) value;
                contextValue.SetValue(data);
                return;
            }

            contextValue = ClassPool.Spawn<ContextValue<TData>>();
            contextValue.SetValue(data);
            _contextValues[type] = contextValue;
         
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

        public void Move<TValue>(TValue value)
        {
            Add<TValue>(value);
        }
    }
}
