namespace Assets.UniGame.ECS.Service
{
    using global::UniGame.ECS.Mono;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = "Game/Ecs/" + nameof(EcsServiceConfiguration),fileName = nameof(EcsServiceConfiguration))]
    public class EcsServiceConfiguration : ScriptableObject
    {
        public AssetReferenceT<EcsSystemConfigurationAsset> systemsConfiguration;
    }
}
