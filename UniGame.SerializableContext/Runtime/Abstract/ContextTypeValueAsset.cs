namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using AssetTypes;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public class ContextTypeValueAsset<TValue,TApiValue> : 
        TypeValueDefaultAsset<TValue,TApiValue>,
        IAsyncContextDataSource
        where TValue : class, TApiValue, new()
    {
        #region inspector
        /// <summary>
        /// create instance of SO to prevent original data changes
        /// </summary>
        public bool createSourceInstance = true;
        
        #endregion
        
        private ContextTypeValueAsset<TValue, TApiValue> sourceValue = null;
        
        public virtual async UniTask<IContext> RegisterAsync(IContext context)
        {
            sourceValue = createSourceInstance ? 
                sourceValue == null ? 
                    Instantiate(this) : sourceValue : 
                this;

            var value = sourceValue.Value;
            context.Publish(value);
            if (value is IAsyncContextDataSource dataSource) {
                await dataSource.RegisterAsync(context);
            }
            
            return context;
        }
    }
    
    public class ContextTypeValueAsset<TValue> : 
        ContextTypeValueAsset<TValue, TValue> 
        where TValue : class, new() { }
}
