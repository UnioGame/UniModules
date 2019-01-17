using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Modules.Tools.UnityTools.Extension;
using UniRx;
using UnityEngine;
using UnityTools.Common;
using UnityTools.Interfaces;
using XNode;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : 
        IContextData<IContext>,
        IContextPublisherProvider<IContext>
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string Name;
        
        #endregion

        public UniPortValue()
        {
            Initialize();
        }
        
        #region private property

        [NonSerialized]
        private bool _initialized = false;
        
        private Dictionary<IContext, IMessagePublisher> _writers;

        private ReactiveContextData<IContext> _data;

        #endregion

        public int Count => _data.Count;
        
        public IReadOnlyCollection<IContext> Contexts => _data.Contexts;

        public void Initialize()
        {
            if (_initialized)
                return;
            _data = new ReactiveContextData<IContext>();
            _writers = new Dictionary<IContext, IMessagePublisher>();
        }
        
        public void CopyTo(IContext context, IMessagePublisher writer )
        {
            _data.CopyTo(context,writer);
        }
        
        public TData Get<TData>(IContext context)
        {
            return _data.Get<TData>(context);
        }

        public bool RemoveContext(IContext context)
        {
            return _data.RemoveContext(context);
        }

        public bool Remove<TData>(IContext context)
        {
            return _data.Remove<TData>(context);
        }

        public void UpdateValue<TData>(IContext context, TData value)
        {
            _data.UpdateValue(context, value);
        }

        public bool HasValue(IContext context, Type type)
        {
            return _data.HasValue(context, type);
        }

        public bool HasValue<TValue>(IContext context)
        {
            return _data.HasValue<TValue>(context);
        }

        public bool HasContext(IContext context)
        {
            return _data.HasContext(context);
        }

        public void ConnectToPort(NodePort port)
        {
            Name = port.fieldName;
        }

        public void Release()
        {
            _data.Release();
        }
        
        public IMessagePublisher GetPublisher(IContext context)
        {
            var writers = _writers;
            if (!writers.TryGetValue(context, out var writer))
            {
                var contextWriter = ClassPool.Spawn<ContextPublisher>();
                contextWriter.Initialize(context,this);
                
                writer = contextWriter;
                writers[context] = writer;
            }

            return writer;
        }

        
    }
}