using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Modules.Tools.UnityTools.Extension;
using UniRx;
using UnityEngine;
using UnityTools.Common;
using XNode;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : 
        IContextData<IContext>,
        IContextWriterProvider<IContext>
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string Name;
        
        #endregion
        
        #region private property

        private ReactiveContextData<IContext> _contextObservers;

        private Dictionary<IContext, IDataWriter> _writers;

        private ReactiveContextData<IContext> _data;

        protected ContextData<IContext> ContextObservers
        {
            get
            {
                if (_contextObservers == null)
                {
                    _contextObservers = new ReactiveContextData<IContext>();
                }
                return _contextObservers;
            }
        }

        protected Dictionary<IContext, IDataWriter> Writers
        {
            get
            {
                if (_writers == null)
                {
                    _writers = new Dictionary<IContext, IDataWriter>();
                }

                return _writers;
            }
        }

        protected ContextData<IContext> Value
        {
            get
            {
                if(_data == null)
                    _data = new ReactiveContextData<IContext>();
                return _data;
            }
        }

        #endregion
                   
        public IReadOnlyCollection<IContext> Contexts => Value.Contexts;

        #region rx 

        public IDisposable Subscribe<TData>(IContext context, Action<TData> observer)
        {

            var subject = GetSubject<TData>(context);

            return subject.Subscribe(observer);
            
        }
        
        #endregion
          
        public void CopyTo(IContext context, IDataWriter writer )
        {
            Value.CopyTo(context,writer);
        }
        
        public TData Get<TData>(IContext context)
        {
            return Value.Get<TData>(context);
        }

        public bool RemoveContext(IContext context)
        {
            FireContextValue<object>(context,null);
            return Value.RemoveContext(context);
        }

        public bool Remove<TData>(IContext context)
        {
            if (Value.Remove<TData>(context))
            {
                FireContextValue<TData>(context,default(TData));
                return true;
            }

            return false;
        }

        public void UpdateValue<TData>(IContext context, TData value)
        {
            FireContextValue(context,value);
            Value.UpdateValue(context, value);
        }

        public bool HasValue<TValue>(IContext context)
        {
            return Value.HasValue<TValue>(context);
        }

        public bool HasContext(IContext context)
        {
            return Value.HasContext(context);
        }

        public void ConnectToPort(NodePort port)
        {
            Name = port.fieldName;
        }

        public void Release()
        {
            ContextObservers.Release();
            Value.Release();
        }
        
        public IDataWriter GetWriter(IContext context)
        {
            var writers = Writers;
            if (!writers.TryGetValue(context, out var writer))
            {
                var contextWriter = ClassPool.Spawn<ContextWriter>();
                contextWriter.Initialize(context,this);
                
                writer = contextWriter;
                writers[context] = writer;
            }

            return writer;
        }
        
        #region private methods
        
        protected void FireContextValue<TData>(IContext context, TData value)
        {
            var subject = ContextObservers.GetOrCreateDefault<Subject<TData>>() GetSubject<TData>(context);
            subject.OnNext(value);
        }

        #endregion

    }
}