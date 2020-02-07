namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using AssetTypes;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public class TypeValueSource<TValue,TApiValue> : 
        TypeValueDefaultAsset<TValue,TApiValue>,
        ISourceValue<TApiValue>
        where TValue : class, TApiValue, new()
    {
        #region inspector
        
        /// <summary>
        /// create instance of SO to prevent original data changes
        /// </summary>
        public bool createSourceInstance = true;
        
        #endregion
        
        private TypeValueSource<TValue, TApiValue> sourceValueSource = null;
        
        public virtual async UniTask<IContext> RegisterAsync(IContext context)
        {
            sourceValueSource = createSourceInstance ? 
                sourceValueSource == null ? Instantiate(this) : sourceValueSource : 
                this;

            var value = sourceValueSource.Value;
            context.Publish(value);
            if (value is IAsyncContextDataSource dataSource) {
                await dataSource.RegisterAsync(context);
            }
            
            return context;
        }
        
    }
    
    public class TypeValueSource<TValue> : 
        TypeValueSource<TValue, TValue> 
        where TValue : class, new() { }
}
