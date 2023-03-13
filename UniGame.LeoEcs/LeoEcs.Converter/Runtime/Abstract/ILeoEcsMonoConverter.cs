namespace UniGame.LeoEcs.Converter.Runtime
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;

    public interface ILeoEcsMonoConverter
    {
        UniTask<EcsPackedEntity> Convert();
        EcsPackedEntity Convert(EcsWorld world);
    }
}