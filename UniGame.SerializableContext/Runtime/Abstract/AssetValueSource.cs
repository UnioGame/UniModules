namespace UniModules.UniGame.SerializableContext.Runtime.Abstract
{
    using Core.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public abstract class AssetValueSource: ScriptableObject,
        IAsyncSource, 
        IAsyncSourcePrototype
    {
        #region inspector

        private LifeTimeDefinition lifeTime;

        #endregion
        
        public ILifeTime LifeTime => lifeTime.LifeTime;

        public abstract UniTask<IContext> RegisterAsync(IContext context);

        public virtual IAsyncSource Create() => Instantiate(this);
        
        protected void OnEnable()
        {
            lifeTime = new LifeTimeDefinition();
        }

        protected void OnDisable()
        {
            //end of value LIFETIME
            lifeTime.Terminate();
            
            if(Application.isPlaying) 
                GameLog.Log($"{nameof(AssetValueSource)}: {GetType().Name} {name} : END OF LIFETIME");
        }
    }
}
