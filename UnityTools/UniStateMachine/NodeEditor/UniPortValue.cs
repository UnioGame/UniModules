using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
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

        private ContextData<IContext> _contextSubjects;

        private Dictionary<IContext, IDataWriter> _writers;

        private ContextData<IContext> _data;

        protected ContextData<IContext> ContextSubjects
        {
            get
            {
                if (_contextSubjects == null)
                {
                    _contextSubjects = new ContextData<IContext>();
                }
                return _contextSubjects;
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
                    _data = new ContextData<IContext>();
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
            ContextSubjects.Release();
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

        protected Subject<TData> GetSubject<TData>(IContext context)
        {
            var subjects = ContextSubjects;
            var subject = subjects.Get<Subject<TData>>(context);
            if (subject == null)
            {
                subject = new Subject<TData>();
                subjects.UpdateValue(context,subject);
            }

            return subject;
        }
        
        protected void FireContextValue<TData>(IContext context, TData value)
        {
            var subject = GetSubject<TData>(context);
            subject.OnNext(value);
        }

        #endregion

    }
}