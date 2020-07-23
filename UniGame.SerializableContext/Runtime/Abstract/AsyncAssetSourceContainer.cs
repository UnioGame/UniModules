namespace UniModules.UniGame.SerializableContext.Runtime.Abstract
{
    using Cysharp.Threading.Tasks;
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    

    public class AsyncAssetSourceContainer<TValue> :
        IAsyncContextDataSource
        where TValue : class
    {
        private ISourceValue<TValue> valueSource = null;
        private ISourceValue<TValue> instance    = null;
        private bool                 createSource;

        public IAsyncContextDataSource Initialize(ISourceValue<TValue> value, bool createInstance = true)
        {
            this.valueSource  = value;
            this.createSource = createInstance;
            return this;
        }
        
        public async UniTask<IContext> RegisterAsync(IContext context)
        {
            var source = instance != null ?
                instance :
                createSource ? 
                    valueSource.Create() : 
                    valueSource;

            var value = source?.Value;
            
            if (value is IAsyncContextDataSource dataSource) {
                await dataSource.RegisterAsync(context);
            }
            else {
                context.Publish(value);
            }
            
            return context;
        }
    }
}