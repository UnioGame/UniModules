using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : IContextData<IContext>
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string Name;
        
        #endregion
        
        #region private property
        
        private ContextDataProvider<IContext> _dataProvider;
        
        protected ContextDataProvider<IContext> Value
        {
            get
            {
                if(_dataProvider == null)
                    _dataProvider = new ContextDataProvider<IContext>();
                return _dataProvider;
            }
        }

        #endregion
                   
        public IReadOnlyCollection<IContext> Contexts => _dataProvider.Contexts;

        #region rx 

        public IDisposable Subscribe<TData>(IContext context, IObserver<TData> observer)
        {
            var subject = Value.Get<Subject<TData>>(context);
            
            if (subject == null)
            {
                subject = new Subject<TData>();
                Value.AddValue(context, subject);
            }

            return subject.Subscribe(observer);
            
        }
        
        #endregion
        
        public TData Get<TData>(IContext context)
        {
            return Value.Get<TData>(context);
        }

        public bool RemoveContext(IContext context)
        {
            return Value.RemoveContext(context);
        }

        public bool Remove<TData>(IContext context)
        {
            var subject = Value.Get<Subject<TData>>(context);
            if (subject != null)
            {
                subject.OnNext(default(TData));
            }
            return Value.Remove<TData>(context);
        }

        public bool AddValue<TData>(IContext context, TData value)
        {
            throw new NotImplementedException();
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
            Value.Release();
        }
        
        #region private methods

        protected void FireContextValue<T>(IContext context, T value)
        {
            var subject = Value.Get<Subject<T>>(context);
            subject?.OnNext(value);
        }

        #endregion
    }
}