namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniGame.ViewSystem.Runtime;

    public interface IEcsViewModel : IViewModel
    {
        UniTask InitializeAsync(EcsWorld world, IContext context);
    }
}