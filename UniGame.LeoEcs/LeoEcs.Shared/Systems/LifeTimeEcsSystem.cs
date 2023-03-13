using Leopotam.EcsLite;
using UniGame.Core.Runtime;
using UniModules.UniCore.Runtime.DataFlow;
using UnityEngine;

namespace UniGame.LeoEcs.Shared.Systems
{
    public class LifeTimeEcsSystem : IEcsInitSystem,IEcsDestroySystem,ILifeTimeContext
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public ILifeTime LifeTime => _lifeTime;
        
        public void Init(EcsSystems systems)
        {
            _lifeTime.Release();
            OnInit(systems,_lifeTime);
        }

        public void Destroy(EcsSystems systems)
        {
            OnDestroy(systems);
            _lifeTime.Release();
        }

        protected virtual void OnInit(EcsSystems systems, ILifeTime lifeTime)
        {
            
        }
        
        protected virtual void OnDestroy(EcsSystems systems)
        {
            
        }
    }
}
