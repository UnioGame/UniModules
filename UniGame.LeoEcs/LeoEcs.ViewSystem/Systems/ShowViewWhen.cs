namespace Game.Ecs.UI.EndGameScreens.Systems
{
    using System;
    using Leopotam.EcsLite;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.ViewSystem.Runtime;
    using Unity.IL2CPP.CompilerServices;
    
    /// <summary>
    /// await target event and create view
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class ShowViewWhen<TView> : IEcsInitSystem, IEcsRunSystem
        where TView : IView
    {
        private ViewRequestData _data;
        private EcsWorld _world;
        private EcsFilter _eventFilter;

        public ShowViewWhen(EcsFilter eventFilter,ViewRequestData data)
        {
            _eventFilter = eventFilter;
            _data = data;
        }
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _world.AddComponent<CreateViewRequest>(requestEntity);

                var parent = _data.Parent;
                if (parent != null)
                {
                    var ecsConverter = parent.gameObject.GetComponent<LeoEcsMonoConverter>();
                    if (ecsConverter != null && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.EntityId;
                }

                requestComponent.Parent = parent;
                requestComponent.Type = typeof(TView);
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
            }
        }
    }
    
    
    /// <summary>
    /// await target event and create view
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class ShowViewWhen<TEvent,TView> : IEcsInitSystem, IEcsRunSystem
        where TEvent : struct
        where TView : IView
    {
        private ViewRequestData _data;
        private EcsWorld _world;
        private EcsFilter _eventFilter;

        public ShowViewWhen(ViewRequestData data)
        {
            _data = data;
        }
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _eventFilter = _world.Filter<TEvent>().End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _world.AddComponent<CreateViewRequest>(requestEntity);

                var parent = _data.Parent;
                if (parent != null)
                {
                    var ecsConverter = parent.gameObject.GetComponent<LeoEcsMonoConverter>();
                    if (ecsConverter != null && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.EntityId;
                }

                requestComponent.Parent = parent;
                requestComponent.Type = typeof(TView);
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
            }
        }
    }
    
    /// <summary>
    /// await target event and create view
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class ShowViewWhen<TEvent1,TEvent2,TView> : IEcsInitSystem, IEcsRunSystem
        where TEvent1 : struct
        where TEvent2 : struct
        where TView : IView
    {
        private ViewRequestData _data;
        private EcsWorld _world;
        private EcsFilter _eventFilter;

        public ShowViewWhen(ViewRequestData data)
        {
            _data = data;
        }
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _eventFilter = _world.Filter<TEvent1>()
                .Inc<TEvent2>()
                .End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _world.AddComponent<CreateViewRequest>(requestEntity);

                var parent = _data.Parent;
                if (parent != null)
                {
                    var ecsConverter = parent.gameObject.GetComponent<LeoEcsMonoConverter>();
                    if (ecsConverter != null && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.EntityId;
                }

                requestComponent.Parent = parent;
                requestComponent.Type = typeof(TView);
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
            }
        }
    }
}