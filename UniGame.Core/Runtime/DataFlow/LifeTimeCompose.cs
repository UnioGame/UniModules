using System;

namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using System.Threading;
    using Interfaces;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;

    public class LifeTimeCompose : IDisposable
    {
        private int _counter;
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        #region lifetime api
        
        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTime.AddRef(o);

        public bool IsTerminated => _lifeTime.IsTerminated;  

        #endregion
        
        public void Add(params ILifeTime[] lifeTimes)
        {
            foreach (var lifeTime in lifeTimes) {
                Add(lifeTime);
            }
        }
        
        public void Add(ILifeTime lifeTime)
        {
            Interlocked.Increment(ref _counter);
            lifeTime.AddDispose(this);
        }
        
        public void Dispose()
        {
            Interlocked.Decrement(ref _counter);
            _lifeTime.Terminate();
            
            if (_counter > 0) {
                return;
            }
            
            //despawn when counter <= 0
            ClassPool.Despawn(this);
        }

    }
}
