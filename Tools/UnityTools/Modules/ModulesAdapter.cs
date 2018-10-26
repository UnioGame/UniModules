using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;

namespace Assets.Tools.UnityTools.Moduls
{
    public class ModulesAdapter : IDisposable
    {
        private readonly IContext _source;

        public ModulesAdapter(IContext source)
        {
            _source = source;
        }

        public void Register()
        {
            
        }

        public void Dispose()
        {
            
        }
        
        protected virtual void BindDependency<TSource,TTarget>(Func<TSource,TTarget> converter)
        {

            _source.Observable<TSource>().
                Subscribe(x => OnContextDataReceived(converter(x)));

        }

        private void OnContextDataReceived<TData>(TData data)
        {
            
        }

    }
}
