namespace UniGame.Core.Runtime.DataFlow
{
    using System;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public class LifeTimeContextCommand : IDisposableCommand,IPoolable
    {
        private LifeTimeDefinition _lifeTime;
        private Action<ILifeTime> _action;

        public void Initialize(Action<ILifeTime> contextAction)
        {
            _lifeTime = ClassPool.Spawn<LifeTimeDefinition>();
            _lifeTime.Release();
            _action = contextAction;
        }

        public void Execute()
        {
            _lifeTime.Release();
            _action?.Invoke(_lifeTime);
        }

        public void Dispose() => this.Despawn();

        public void Release()
        {
            ClassPool.Despawn(ref _lifeTime);
            _action = null;
        }
    }
}
