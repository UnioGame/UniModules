using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.RecycleRx;
using UniRx;

namespace UniModule.UnityTools.Common
{
    [Serializable]
    public class ContextValue<TData> : IDataValue<TData>
    {
        private bool _hasValue = false;
        
        protected RecycleObservable<TData> _reactiveValue = new RecycleObservable<TData>();

        public TData Value
        {
            get => _reactiveValue.Value;
            private set => _reactiveValue.SetValue(value);
        }

        public bool HasValue()
        {
            return _hasValue;
        }
        
        public void SetValue(TData value)
        {
            _hasValue = true;
            Value = value;
        }

        public void Dispose()
        {
            _hasValue = false;
            _reactiveValue.Release();
            
            OnRelease();
            
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }
               
        protected virtual void OnRelease(){}

    }
}