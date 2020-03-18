namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool.Runtime.Extensions;
    using ObjectPool.Runtime.Interfaces;
    using UniGame.Core.Runtime.Rx;

    [Serializable]
    public class ContextValue<TData> : IDataValue<TData> , IPoolable
    {
        private bool hasValue = false;
        private bool isValueType = typeof(TData).IsValueType;
        
        protected RecycleReactiveProperty<TData> _reactiveValue = new RecycleReactiveProperty<TData>();

        public TData Value
        {
            get => _reactiveValue.Value;
            private set => _reactiveValue.SetValue(value);
        }

        public bool HasValue => hasValue;

        public bool IsValueType => isValueType;
        
        public void SetValue(TData value)
        {
            hasValue = true;
            Value = value;
        }

        public void Dispose() => this.Despawn();

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }

        public void Release()
        {
            hasValue = false;
            _reactiveValue.Release();
        }
    }
}