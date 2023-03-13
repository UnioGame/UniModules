namespace UniGame.LeoEcs.Shared.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Unity.IL2CPP.CompilerServices;

    /// <summary>
    /// make component trigger on some event
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class FireOnSystem<TFilter,TComponent> : IEcsInitSystem, IEcsRunSystem
        where TFilter : struct
        where TComponent : struct
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TComponent> _pool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<TFilter>().End();
            _pool = _world.GetPool<TComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var eventEntity = _world.NewEntity();
                _pool.Add(eventEntity);
            }
        }
    }
    
    /// <summary>
    /// make component trigger on some event
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class FireOnSystem<TFilter1,TFilter2,TComponent> : IEcsInitSystem, IEcsRunSystem
        where TFilter1 : struct
        where TFilter2 : struct
        where TComponent : struct
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TComponent> _pool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<TFilter1>()
                .Inc<TFilter2>()
                .End();
            
            _pool = _world.GetPool<TComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var eventEntity = _world.NewEntity();
                _pool.Add(eventEntity);
            }
        }
    }
    
    /// <summary>
    /// make component trigger on some event
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class FireOnSystem<TFilter1,TFilter2,TFilter3,TComponent> : IEcsInitSystem, IEcsRunSystem
        where TFilter1 : struct
        where TFilter2 : struct
        where TFilter3 : struct
        where TComponent : struct
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<TComponent> _pool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<TFilter1>()
                .Inc<TFilter2>()
                .Inc<TFilter3>()
                .End();
            
            _pool = _world.GetPool<TComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var eventEntity = _world.NewEntity();
                _pool.Add(eventEntity);
            }
        }
    }
}