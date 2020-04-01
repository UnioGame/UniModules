namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Abstract;
    using UnityEngine;

    public class DisposableScriptableObject : ScriptableObject,
        ILifeTimeContext,
        IResourceDisposable
    {
        private LifeTimeDefinition _lifeTimeDefinition;
        
        public ILifeTime LifeTime => _lifeTimeDefinition;

        public void Dispose()
        {
            if (_lifeTimeDefinition.IsTerminated)
                return;
            _lifeTimeDefinition.Terminate();
        }

        private void OnEnable()
        {
            _lifeTimeDefinition = new LifeTimeDefinition();
            OnSourceEnable(LifeTime);
        }

        private void OnDisable()
        {
            Dispose();
        }

        protected virtual void OnSourceEnable(ILifeTime lifeTime) {}
    }
}