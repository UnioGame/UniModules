using System;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;

namespace Assets.Tools.UnityTools.Moduls
{
    public class ModulesAdapter : IDisposable
    {
        private readonly IContext _source;
        private readonly IContext _target;

        public ModulesAdapter(IContext source, IContext target)
        {
            _source = source;
            _target = target;
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
            _target.Add(data);
        }

    }
}
