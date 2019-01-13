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
        
        public IDisposable Subscribe<TData>(TContext context, Action<TData> observer)
        {

            var subject = 

            return subject.Subscribe(observer);
            
        }
        
        #endregion

    }
}
