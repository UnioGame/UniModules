namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Runtime.Extensions;
    using Rx;
    using UniGame.Core.Runtime.Rx;

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

        public bool HasValue => _hasValue;
        
        
        public void SetValue(TData value)
        {
            _hasValue = true;
            Value = value;
        }

        public void Dispose()
        {
            _hasValue = false;
            _reactiveValue.Release();
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }

    }
}