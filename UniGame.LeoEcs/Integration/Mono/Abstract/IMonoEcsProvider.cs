namespace UniGame.ECS.Mono.Abstract
{
    using Leopotam.EcsLite;

    public interface IMonoEcsProvider
    {
        int CreateWorldEntity(EcsWorld world);
    }
}