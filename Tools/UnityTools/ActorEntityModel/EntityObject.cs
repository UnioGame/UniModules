﻿using System;
using System.Collections.Generic;
using Assets.Tools.Utils;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniTools.Common;

namespace Tools.ActorModel
{
    public class EntityObject : IEntity
    {
        /// <summary>
        /// registered conmponents
        /// </summary>
        private Dictionary<Type, object> _contextValues = new Dictionary<Type, object>();
        
        #region public properties
        
        public int Id { get; protected set; }

        #endregion

        #region public methods

        public void Dispose()
        {
            _contextValues.Clear();
            Id = -1;
            OnDispose();
        }

        public virtual TData GetContext<TData>() where TData : class
        {
            
            object value = null;
            if (!_contextValues.TryGetValue(typeof(TData), out value))
            {
                return null;
            }

            var valueData = value as IDataValue<TData>;
            return valueData?.Value;
            
        }
              
        public void AddContext<TData>(TData data)
        {
            object value = null;
            DataValue<TData> dataValue = null;
            var type = typeof(TData);
            
            if (_contextValues.TryGetValue(type, out value))
            {
                dataValue = value as DataValue<TData>;
                dataValue.SetValue(data);
                return;
            }
            
            dataValue = ClassPool.Spawn<DataValue<TData>>();
            dataValue.SetValue(data);
            _contextValues[type] = dataValue;

        }
        
        #endregion

        #region private methods

        protected virtual void OnDispose()
        {
        }

        #endregion
    }
}