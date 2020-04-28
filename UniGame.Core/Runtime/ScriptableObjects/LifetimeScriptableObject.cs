namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class LifetimeScriptableObject : ScriptableObject, 
        ILifeTimeContext
    {
        private static Color _logColor = new Color(0.290f, 0.490f, 0.290f);
        
        protected LifeTimeDefinition _lifeTimeDefinition;
        
        public ILifeTime LifeTime => _lifeTimeDefinition;

        public void Reset()
        {
            if(_lifeTimeDefinition != null) {
                _lifeTimeDefinition.Release();
            }
            OnReset();
        }
        
        private void OnEnable()
        {
            GameLog.Log($"LIFETIME: {GetType().Name} {name} : OnEnable STARTED",_logColor,this);
            _lifeTimeDefinition = new LifeTimeDefinition();
            OnActivate();
        }

        private void OnDisable()
        {
//            if (!Application.isPlaying)
//                return;
            GameLog.Log($"LIFETIME: {GetType().Name} {name} : OnDisable END OF LIFETIME",_logColor,this);
            _lifeTimeDefinition.Terminate();
        }

        protected virtual void OnActivate() {}

        protected virtual void OnReset() {}
    }
}