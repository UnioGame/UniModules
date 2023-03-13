namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System.Threading;
    using Components;
    using Converter.Runtime;
    using Core.Runtime;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniCore.Runtime.DataFlow;
    using UnityEngine;

    [RequireComponent(typeof(LeoEcsMonoConverter))]
    public class EcsViewConverter : MonoLeoEcsConverter, IEcsViewConverter
    {
        private bool _isEntityAlive;
        private EcsWorld _ecsWorld;
        private EcsPackedEntity _viewPackedEntity;
        private IView _view;
        
        public int entityId;

        #region public properties
        public bool IsEnabled => isActiveAndEnabled;
        public bool IsEntityAlive => _isEntityAlive;
        public EcsWorld World => _ecsWorld;
        public EcsPackedEntity PackedEntity => _viewPackedEntity;
        public int Entity => entityId;
        
        #endregion
        
        #region public methods

        /// <summary>
        /// entity destroyed
        /// </summary>
        public void OnEntityDestroy(EcsWorld world, int entity)
        {
            _isEntityAlive = false;
            _ecsWorld = null;
            
            entityId = -1;
        }
        
        public sealed override void Apply(GameObject target, EcsWorld world, int entity, 
            CancellationToken cancellationToken = default)
        {
            _view = GetComponent<IView>();
            
            if (!isActiveAndEnabled || _view == null) return;

            _ecsWorld = world;
            _viewPackedEntity = world.PackEntity(entity);
            
            entityId = entity;
            
            ref var viewComponent = ref world.GetOrAddComponent<ViewComponent>(entity);
            ref var viewStatusComponent = ref world.GetOrAddComponent<ViewStatusComponent>(entity);

            viewComponent.View = _view;
            viewComponent.Type = _view.GetType();

            _isEntityAlive = true;
        }

        #endregion
    }
}