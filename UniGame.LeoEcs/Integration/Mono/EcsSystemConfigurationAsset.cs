namespace UniGame.ECS.Mono
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game/Ecs/" + nameof(EcsSystemConfigurationAsset),fileName = nameof(EcsSystemConfigurationAsset))]
    public class EcsSystemConfigurationAsset : ScriptableObject
    {

        [InlineProperty]
        [HideLabel]
        [TitleGroup(nameof(configuration))]
        public EcsSystemConfiguration configuration = new EcsSystemConfiguration();

    }
}
