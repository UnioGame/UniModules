namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsLite;
    using Runtime.ObjectPool;
    using Shared.Components;
    using UnityEngine;

    [Serializable]
    public class GameObjectEntityBuilder : IEntityEditorViewBuilder
    {
        private EcsWorld _world;
        private EcsFilter _gameObjectFilter;
        private EcsPool<GameObjectComponent> _gameObjectPool;

        public void Initialize(EcsWorld world)
        {
            _world = world;
            _gameObjectFilter = world.Filter<GameObjectComponent>().End();
            _gameObjectPool = _world.GetPool<GameObjectComponent>();
        }

        public void Execute(List<EntityEditorView> views)
        {
            foreach (var view in views)
            {
                foreach (var entity in _gameObjectFilter)
                {
                    if (view.id != entity) continue;
                    ref var gameObjectComponent = ref _gameObjectPool.Get(entity);
                    var gameObject = gameObjectComponent.GameObject;
                    if(gameObject == null) continue;
                    
                    view.gameObject = gameObject;
                    view.name = gameObject.name;
                }
            }
        }
    }
}