using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using System;
    using Interfaces;
    using UniGreenModules.UniCore.Runtime.DataFlow;

    public class LifeTimeComponent :
        MonoBehaviour, 
        ILifeTime
    {
        private readonly LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTime.AddRef(o);

        public bool IsTerminated => _lifeTime.IsTerminated;

        private void OnDestroy()
        {
            _lifeTime.Terminate();
        }
    }
}
