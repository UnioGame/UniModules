namespace UniModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;
    using Abstract;
    using Context.Runtime.Abstract;
    using Cysharp.Threading.Tasks;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.Core.Runtime.Rx;
    using UniRx;
    

    public class TypeContainerAssetSource<TValue> :
        TypeContainerAssetSource<TValue, TValue>
    {
    }

    public class TypeContainerAssetSource<TValue,TApi> : 
        AsyncContextDataSource, 
        IDataValue<TValue,TApi> 
        where TValue : TApi
    {
        private RecycleReactiveProperty<TValue> _value;
        
        public TApi Value => _value.Value;

        public bool HasValue => _value != null && _value.HasValue;
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            await UniTask.WaitWhile(() => HasValue == false);
            context.Publish(Value);
            return context;
        }

        public IDisposable Subscribe(IObserver<TApi> observer) =>
            _value.Subscribe(api => observer.OnNext(api), observer.OnError, observer.OnCompleted);

        public void Dispose() => Reset();

        public void SetValue(TValue value) => _value.Value = value;
      
        #region private methods
        
        protected override void OnActivate()
        {
            base.OnActivate();
            _value = new RecycleReactiveProperty<TValue>();
            LifeTime.AddCleanUpAction(_value.Release);
        }

        protected sealed override void OnReset()
        {
            base.OnReset();
            LifeTime.AddCleanUpAction(_value.Release);
            ResetValue();
        }

        protected virtual void ResetValue()
        {
            
        }

        #endregion
        
    }
}
