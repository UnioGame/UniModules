using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using UniRx;
using UnityEngine;

namespace UnityTools.Common
{
    public class ReactiveContextData<TContext> : ContextData<TContext>
    {
        private ContextData<TContext> _subjects;
        
        #region reactive methods
        
        public IDisposable Subscribe<TData>(TContext context, IObserver<TData> observer)
        {
            var container = GetTypeData(context);
            return container.Subscribe<TData>(observer);
        }
        
        public IDisposable SubscribeOnContext(IObserver<TContext> observer)
        {
            return Disposable.Empty;
        }
        
        #endregion

    }
}
