namespace UniGreenModules.UniGame.Context.Runtime.Interfaces
{
    using SerializableContext.Runtime.Abstract;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class DisposableScriptableObject : ScriptableObject,
        ILifeTimeContext,
        IResourceDisposable
    {
        #region inspector
        
        [Tooltip("Unload context asset on SO unloading")]
        public bool disposeOnUnload = true;
        
        #endregion
        
        private LifeTimeDefinition lifeTimeDefinition;
        
        public ILifeTime LifeTime => lifeTimeDefinition;
        
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