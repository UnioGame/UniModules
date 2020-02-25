namespace UniGreenModules.UniGame.Context.Runtime.Interfaces
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public abstract class AsyncContextDataSource : 
        ScriptableObject, 
        IAsyncContextDataSource,
        ILifeTimeContext
    {
        private LifeTimeDefinition lifeTimeDefinition;
        
        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        

        public abstract UniTask<IContext> RegisterAsync(IContext context);


        private void OnEnable()
        {
            lifeTimeDefinition = new LifeTimeDefinition();
            OnSourceEnable(LifeTime);
        }

        private void OnDisable() => lifeTimeDefinition.Terminate();

        protected virtual void OnSourceEnable(ILifeTime lifeTime) {}
    }
}
