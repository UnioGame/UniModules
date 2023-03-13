namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class LeoEcsGizmosExecutor : MonoBehaviour,ISystemsPlugin
    {
        private bool _isActive;
        private EcsWorld _world;
        private GameObject _executor;
        
        private List<EcsSystems> _systems = new List<EcsSystems>();
        private IEcsSystem[] _allSystems = Array.Empty<IEcsSystem>();
        private Dictionary<ILeoEcsGizmosSystem,EcsSystems> _gizmosSystems = new Dictionary<ILeoEcsGizmosSystem,EcsSystems>();
        
        public void Dispose()
        {
            _gizmosSystems?.Clear();
            Stop();
            Destroy(gameObject);
        }

        public bool IsActive => _isActive;

        public void Execute(EcsWorld world)
        {
            _isActive = true;
            _world = world;
        }

        public void Add(EcsSystems ecsSystems)
        {
#if !UNITY_EDITOR
            return; 
#endif
            if (_systems.Contains(ecsSystems))
                return;

            _systems.Add(ecsSystems);
            ecsSystems.GetAllSystems(ref _allSystems);
            foreach (var system in _allSystems)
            {
                if (system is ILeoEcsGizmosSystem gizmosSystem)
                    _gizmosSystems[gizmosSystem] = ecsSystems;
            }
        }

        public void Stop()
        {
            _isActive = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!this) return;
            
            var isActive = _world!=null && 
                           _world.IsAlive() && 
                           Application.isPlaying && 
                           _isActive;
            
            if (!isActive)
                return;
            
            foreach (var systemValue in _gizmosSystems)
            {
                systemValue.Key.RunGizmosSystem(systemValue.Value);
            }
        }
#endif

        private void Awake()
        {
            _systems ??= new List<EcsSystems>();
            _allSystems ??= Array.Empty<IEcsSystem>();
            _gizmosSystems ??= new Dictionary<ILeoEcsGizmosSystem, EcsSystems>();
        }
    }
}