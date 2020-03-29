namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using AssetTypes;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx.Async;

    public class TypeValueSource<TValue,TApiValue> : 
        TypeValueDefaultAsset<TValue,TApiValue>,
        ISourceValue<TApiValue>
        where TValue : class, TApiValue, new() 
        where TApiValue : class
    {
        /// <summary>
        /// create instance of SO to prevent original data changes
        /// </summary>
        public bool createSourceInstance = true;
        
        private IAsyncContextDataSource sourceValueSource ;
        
        public async UniTask<IContext> RegisterAsync(IContext context)
        {
            sourceValueSource = sourceValueSource ?? 
                                new AsyncAssetSourceContainer<TApiValue>().
                                    Initialize(this, createSourceInstance);
            await sourceValueSource.RegisterAsync(context);
            return context;
        }

        public ISourceValue<TApiValue> Create()
        {
            var value = Instantiate(this);
            //bind child lifetime to asset source
            value.AddTo(LifeTime);
            return value;
        }
    }

    public class TypeValueSource<TValue> : 
        TypeValueSource<TValue, TValue> 
        where TValue : class, new() { }
}
