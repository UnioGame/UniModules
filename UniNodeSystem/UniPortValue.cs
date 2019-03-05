using System;
using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniRx;
using UnityEngine;
using UniNodeSystem;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : ContextData<IContext>, IBroadcastContextData<IContext>
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string Name;
        
        #endregion
        
        #region private property

        [NonSerialized]
        private bool _initialized = false;

        private BroadcastContextData<IContext> _broadcastContext;

        #endregion

        public UniPortValue()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            _contextsItems = new List<IContext>();
            _contexts = new Dictionary<IContext, TypeData>();
            _broadcastContext = new BroadcastContextData<IContext>();

        }
        
        public void ConnectToPort(string portName)
        {
            Name = portName;
        }

        public override bool Remove<TData>(IContext context)
        {
            var result = base.Remove<TData>(context);
            if (result)
            {
                _broadcastContext.Remove<TData>(context);
            }

            return result;
        }

        public override bool RemoveContext(IContext context)
        {
            var result = base.RemoveContext(context);
            if (result)
            {
                _broadcastContext.RemoveContext(context);
            }

            return result;
        }

        public override void UpdateValue<TData>(IContext context, TData value)
        {
            base.UpdateValue(context, value);
            _broadcastContext.UpdateValue(context,value);
        }

        public void Add(IContextDataWriter<IContext> contextData)
        {
            _broadcastContext.Add(contextData);
        }

        public void Remove(IContextDataWriter<IContext> contextData)
        {
            _broadcastContext.Remove(contextData);
        }
    }
}