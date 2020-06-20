namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Abstract;
    using UnityEngine;

    public class DisposableScriptableObject : 
        LifetimeScriptableObject,
        IResourceDisposable
    {
        public void Dispose()
        {
            if (_lifeTimeDefinition.IsTerminated)
                return;
            
            GameLog.Log($"DisposableAsset: {GetType().Name} {name} : DISPOSED",Color.blue,this);
            
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }

    }
}