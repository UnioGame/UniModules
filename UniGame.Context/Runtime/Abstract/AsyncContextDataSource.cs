namespace UniGreenModules.UniGame.Context.Runtime.Interfaces
{
    using SerializableContext.Runtime.Abstract;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public abstract class AsyncContextDataSource : 
        ScriptableObject, 
        IAsyncContextDataSource,
        ILifeTimeContext,
        IResourceDisposable
    {
        #region inspector
        
        [Tooltip("Unload context asset on SO unloading")]
        public bool disposeOnUnload = true;
        
        #endregion
        
        private LifeTimeDefinition lifeTimeDefinition;
        
        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        

        public abstract UniTask<IContext> RegisterAsync(IContext context);
        
        public void Dispose() => lifeTimeDefinition.Terminate();
        
        private void OnEnable()
        {
            lifeTimeDefinition = new LifeTimeDefinition();
            if (disposeOnUnload) {
                lifeTimeDefinition.LifeTime.AddDispose(this);
            }
            OnSourceEnable(LifeTime);
        }

        private void OnDisable() => Dispose();

        protected virtual void OnSourceEnable(ILifeTime lifeTime) {}
        
    }
}
