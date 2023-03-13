namespace UniGame.LeoEcs.ViewSystem
{
    using Behavriour;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.ExtendedSystems;
    using UniGame.Context.Runtime.Extension;
    using UniGame.Core.Runtime;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.ViewSystem.Systems;
    using UniGame.ViewSystem.Runtime;
    using UniGame.LeoEcs.Bootstrap.Runtime.Config;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Feature/Views Feature", fileName = nameof(ViewSystemFeature))]
    public class ViewSystemFeature : LeoEcsFeatureGroupAsset
    {
        private EcsViewTools _ecsViewTools;
        
        protected override async UniTask OnPostInitializeFeatureAsync(EcsSystems ecsSystems)
        {
            var context = ecsSystems.GetShared<IContext>();
            var viewSystem = await context.ReceiveFirstAsync<IGameViewSystem>();
            
            _ecsViewTools = new EcsViewTools(context, viewSystem);
            
            ecsSystems.Add(new CloseViewSystem());
            ecsSystems.Add(new ViewServiceInitSystem(viewSystem));
            ecsSystems.Add(new CloseAllViewsSystem(viewSystem));
            
            ecsSystems.Add(new CreateLayoutViewSystem());
            ecsSystems.DelHere<CreateLayoutViewRequest>();

            ecsSystems.Add(new ViewUpdateStatusSystem());
            ecsSystems.Add(new CreateViewSystem(context,viewSystem,_ecsViewTools));
            ecsSystems.Add(new InitializeViewsSystem(_ecsViewTools));
            ecsSystems.Add(new RemoveUpdateRequest());
            
            ecsSystems.DelHere<CreateViewRequest>();
            ecsSystems.DelHere<CloseAllViewsRequest>();
            ecsSystems.DelHere<CloseViewByTypeRequest>();
            ecsSystems.DelHere<CloseTargetViewByTypeRequest>();
            ecsSystems.DelHere<CloseViewRequest>();
            ecsSystems.DelHere<UpdateViewRequest>();
        }

    }
}
