namespace UniModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;
    using Abstract;
    using Context.Runtime.Abstract;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.Core.Runtime.Rx;
    using UniRx;
    using UniRx.Async;

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
