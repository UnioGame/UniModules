namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using System;
    using Components;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;

    [Serializable]
    public class EcsViewTools : IEcsViewTools
    {
        private readonly IContext _context;
        private readonly IGameViewSystem _viewSystem;
        private readonly ILifeTime _lifeTime;

        public EcsViewTools(IContext context,IGameViewSystem viewSystem)
        {
            _context = context;
            _viewSystem = viewSystem;
            _lifeTime = context.LifeTime;
        }


        public ILifeTime LifeTime => _lifeTime;
        
        public async UniTask AddModelComponentAsync(
            EcsWorld world,EcsPackedEntity packedEntity,
            IView view,Type viewType)
        {
            var modelType = _viewSystem.ModelTypeMap.GetViewModelTypeByView(viewType);
            var model = await _viewSystem.CreateViewModel(_context, modelType);
            
            AddViewModelData(world, packedEntity, model);
            
            await _viewSystem
                .InitializeView(view, model)
                .AttachExternalCancellation(_lifeTime.TokenSource);
        }

        public void AddViewModelData(EcsWorld world,EcsPackedEntity packedEntity,IViewModel model)
        {
            if (!packedEntity.Unpack(world, out var entity)) return;

            ref var modelComponent = ref world.GetOrAddComponent<ViewModelComponent>(entity);
            modelComponent.Model = model;
        }

    }
}