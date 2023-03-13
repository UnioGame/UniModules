namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Leopotam.EcsLite;
    using UnityEngine;


    public class LeoEcsUpdateExecutor : MonoBehaviour,ILeoEcsExecutor
    {
        private bool _isActive;
        private EcsWorld _world;
        
        private List<EcsSystems> _systems = new List<EcsSystems>();
        private IEcsSystem[] _allSystems = Array.Empty<IEcsSystem>();

        public bool IsActive => _isActive;
        
        public void Dispose()
        {
            Stop();
            Destroy(gameObject);
        }

        public void Execute(EcsWorld world)
        {
            _isActive = true;
            _world = world;
        }

        public void Add(EcsSystems ecsSystems)
        {
            if (_systems.Contains(ecsSystems))
                return;

            _systems.Add(ecsSystems);
        }

        public void Stop()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!this) return;
            
            var isActive = _world.IsAlive() && Application.isPlaying && _isActive;
            if (!isActive)
                return;
            
            foreach (var systemValue in _systems)
            {
                systemValue.Run();
            }
        }

        private void Awake()
        {
            _systems ??= new List<EcsSystems>();
            _allSystems ??= Array.Empty<IEcsSystem>();
        }
    }
}