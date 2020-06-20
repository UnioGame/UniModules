namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System;
    using DataFlow.Interfaces;
    using Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UnityEngine;

    public class LifetimeScriptableObject : ScriptableObject, 
        ILifeTime,
        ILifeTimeContext
    {
        private static Color _logColor = new Color(0.30f, 0.8f, 0.490f);
        
        protected LifeTimeDefinition _lifeTimeDefinition;

        #region LifeTime API

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTimeDefinition.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTimeDefinition.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTimeDefinition.AddRef(o);

        public bool IsTerminated => _lifeTimeDefinition.IsTerminated;
        
        #endregion
                
        public ILifeTime LifeTime => _lifeTimeDefinition;

        public void Reset()
        {
            _lifeTimeDefinition?.Release();
            OnReset();
        }
        
        private void OnEnable()
        {
            if (Application.isPlaying) {
                GameLog.Log($"LIFETIME: {GetType().Name} {name} : OnEnable STARTED",_logColor,this);
            }
            
            _lifeTimeDefinition = new LifeTimeDefinition();
#if UNITY_EDITOR
            LifetimeObjectData.Add(this);
#endif
            OnActivate();
        }

        private void OnDisable()
        {
            if (Application.isPlaying) {
                GameLog.Log($"LIFETIME: {GetType().Name} {name} : OnDisable END OF LIFETIME",_logColor,this);
            }
            
#if UNITY_EDITOR
            LifetimeObjectData.Remove(this);
#endif    
            _lifeTimeDefinition.Terminate();
            
        }

        protected virtual void OnActivate() {}

        protected virtual void OnReset() {}

    }
}