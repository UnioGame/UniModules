namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;
    using Abstract;
    using Core.Runtime.Rx;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx.Async;
    using UniRx;

    public class TypeContainerAssetSource<TValue> :
        TypeValueAssetSource<TValue, TValue>
    {
    }

    public class TypeContainerAssetSource<TValue,TApi> : 
        AsyncContextDataSource, 
        IDataValue<TValue,TApi> 
        where TValue : TApi
    {
        private RecycleReactiveProperty<TValue> _value;

        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            await UniTask.WaitWhile(() => HasValue == false);
            context.Publish(Value);
            return context;
        }

        public IDisposable Subscribe(IObserver<TApi> observer) =>
            _value.Subscribe(api => observer.OnNext(api), observer.OnError, observer.OnCompleted);

        public void Dispose() => Reset();

        public TApi Value => _value.Value;
        
        public bool HasValue => _value.HasValue;
        
        public void SetValue(TValue value) => _value.Value = value;
        
        protected override void OnActivate()
        {
            base.OnActivate();
            _value = new RecycleReactiveProperty<TValue>();
            LifeTime.AddCleanUpAction(_value.Release);
        }

        protected override void OnReset()
        {
            base.OnReset();
            _lifeTimeDefinition.AddCleanUpAction(_value.Release);
        }
    }
}
