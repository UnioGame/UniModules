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
        ReactiveContextData<IContext>,
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
            
            _contexts = new Dictionary<IContext, TypeData>();
            _writers = new Dictionary<IContext, IMessagePublisher>();
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