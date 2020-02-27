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

        public void Dispose()
        {
            hasValue = false;
            _reactiveValue.Release();
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }

    }
}