using System;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Modules;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;

namespace Assets.Tools.UnityTools.Moduls
{
    public class ModulesDefinition : IDisposable
    {
        private IContext _source;
        private List<IDisposable> _disposables;

        private List<MessageConverter> _converters;
        
        public ModulesDefinition(IContext source)
        {
            _disposables.Cancel();
            _disposables = new List<IDisposable>();
            _source = source;
        }

        public void Register(IMessageBroker messageBroker)
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
