using Leopotam.EcsLite;

namespace UniGame.LeoEcs.Converter.Runtime
{
    public interface IConverterEntityDestroyHandler
    {

        void OnEntityDestroy(EcsWorld world, int entity);

    }
}