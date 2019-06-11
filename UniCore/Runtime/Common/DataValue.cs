namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool;
    using Rx;

    [Serializable]
    public class ContextValue<TData> : IDataValue<TData>
    {
        private bool _hasValue = false;
        
        protected RecycleReactiveProperty<TData> _reactiveValue = new RecycleReactiveProperty<TData>();

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