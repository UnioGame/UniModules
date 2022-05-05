namespace UniGame.ECS.Mono.Abstract
{
    using Leopotam.EcsLite;

    public interface IEcsComponentSource
    {
        public void CreateComponent(EcsWorld world, int entityId);
    }
}
