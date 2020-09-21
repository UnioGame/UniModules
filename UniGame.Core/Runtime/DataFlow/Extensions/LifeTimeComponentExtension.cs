namespace UniModules.UniGame.Core.Runtime.DataFlow.Extensions
{
    using System;
    using Interfaces;
    using UnityEngine;

    public static class LifeTimeComponentExtension 
    {
        public static ILifeTime GetLifeTime(this GameObject gameObject)
        {
            var lifetimeComponent = 
                gameObject.GetComponent<ILifeTime>() ?? 
                gameObject.gameObject.AddComponent<LifeTimeComponent>();
            return lifetimeComponent;
        }

        public static ILifeTime GetLifeTime(this Component component) => component.gameObject.GetLifeTime();
        
        public static ILifeTime AddTo(this GameObject gameObject, IDisposable disposable)
        {
            return gameObject.GetLifeTime().AddDispose(disposable);
        }

        public static ILifeTime AddCleanUp(this GameObject gameObject, Action cleanupAction)
        {
            return gameObject.GetLifeTime().AddCleanUpAction(cleanupAction);
        }
        
        public static ILifeTime AddTo(this Component component, IDisposable disposable) => AddTo(component.gameObject, disposable);
        
        
        public static ILifeTime AddCleanUp(this Component component, Action cleanupAction) => AddCleanUp(component.gameObject, cleanupAction);

    }
}
