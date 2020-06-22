using System;

namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;

    public class LifeTimeCompose : IDisposable
    {
        private int _counter;
        private Action _action;
        
        public void Initialize(Action action, params ILifeTime[] lifeTimes)
        {
            _action = action;
            _counter = lifeTimes.Length;
            
            foreach (var lifeTime in lifeTimes) {
                lifeTime.AddDispose(this);
            }
        }
        
        public void Add(ILifeTime lifeTime)
        {
            _counter++;
            lifeTime.AddDispose(this);
        }
        
        public void Dispose()
        {
            _counter--;
            _action?.Invoke();
            _action = null;
            
            if (_counter > 0) {
                return;
            }
            //despawn when counter <= 0
            ClassPool.Despawn(this);
        }
    }
}
