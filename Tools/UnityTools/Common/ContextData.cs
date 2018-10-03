using System;
using System.Collections.Generic;
using Assets.Tools.Utils;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniTools.Common;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.Common
{
    public class ContextData : IContext
    {
        /// <summary>
        /// registered conmponents
        /// </summary>
        private Dictionary<Type, object> _contextValues = new Dictionary<Type, object>();

        public virtual TData Get<TData>()
        {
            if (!_contextValues.TryGetValue(typeof(TData), out var value))
            {
                return default(TData);
            }

            var valueData = value as IDataValue<TData>;
            return valueData.Value;

        }

        public bool Remove<TData>()
        {
            var type = typeof(TData);
            if (_contextValues.TryGetValue(type, out var value))
            {
                var typeValue = (IDataValue<TData>)value;
                var removed = _contextValues.Remove(type);

                typeValue.Dispose();
                return removed;
            }

            return false;
        }

        public void Add<TData>(TData data)
        {
            object value = null;
            IDataValue<TData> dataValue = null;
            var type = typeof(TData);

            if (_contextValues.TryGetValue(type, out value))
            {
                dataValue = value as IDataValue<TData>;
                dataValue.SetValue(data);
                return;
            }

            dataValue = ClassPool.Spawn<DataValue<TData>>();
            dataValue.SetValue(data);
            _contextValues[type] = dataValue;

        }

        public void Release()
        {

            foreach (var contextValue in _contextValues)
            {
                var disposable = contextValue.Value as IDisposable;
                disposable.Dispose();
            }
            _contextValues.Clear();

        }

    }
}
