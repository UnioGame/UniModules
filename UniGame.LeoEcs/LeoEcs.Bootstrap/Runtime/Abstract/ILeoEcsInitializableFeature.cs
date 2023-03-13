namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;

    public interface ILeoEcsInitializableFeature
    {
        UniTask InitializeFeatureAsync(EcsSystems ecsSystems);
    }
}