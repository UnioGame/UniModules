namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UniGreenModules.UniCore.Runtime.Common;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public class ComposedLifeTime : IPoolable, IComposedLifeTime
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        #region lifetime api

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) =>  _lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) =>  _lifeTime.AddRef(o);

        public bool IsTerminated => _lifeTime.IsTerminated;
        
        #endregion

        public IComposedLifeTime Bind(ILifeTime lifeTime)
        {
            if (IsTerminated) {
                return this;
            }
            
            //weak ref to composite lifetime
            var disposableAction = ClassPool.Spawn<DisposableAction>();
            //release refs if lifetime finished
            _lifeTime.AddCleanUpAction(disposableAction.Complete);
            //bind to target lifetime
            disposableAction.Initialize(Dispose);
            lifeTime.AddDispose(disposableAction);
            
            return this;
        }
        
        public IComposedLifeTime Bind(IEnumerable<ILifeTime> lifeTimes)
        {
            if (IsTerminated) {
                return this;
            }

            foreach (var x in lifeTimes) {
                if (!Bind(x).IsTerminated) {
                    continue;
                }

                Dispose();
                return this;
            }

            return this;
        }
        
        public void Dispose() => _lifeTime.Terminate();
        public void Release() => _lifeTime.Release();
    }
}