namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using System;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniGame.ViewSystem.Runtime;

    public interface IEcsViewTools : ILifeTimeContext
    {
        UniTask AddModelComponentAsync(
            EcsWorld world,EcsPackedEntity packedEntity,
            IView view,Type viewType);

        void AddViewModelData(EcsWorld world,EcsPackedEntity packedEntity,IViewModel model);
    }
}