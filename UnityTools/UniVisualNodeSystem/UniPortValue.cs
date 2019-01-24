using System;
using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : 
        ContextData<IContext>,
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

        #endregion

        public void Initialize()
        {
            if (_initialized)
                return;
            
            _writers = new Dictionary<IContext, IMessagePublisher>();
            _contextsItems = new List<IContext>();
            _contexts = new Dictionary<IContext, TypeData>();
        }

        public void ConnectToPort(NodePort port)
        {
            Name = port.fieldName;
        }
        
        public IMessagePublisher GetPublisher(IContext context)
        {
            if (!_writers.TryGetValue(context, out var writer))
            {
                var contextWriter = ClassPool.Spawn<ContextPublisher>();
                contextWriter.Initialize(context,this);
                
                writer = contextWriter;
                _writers[context] = writer;
            }

            return writer;
        }

        
    }
}