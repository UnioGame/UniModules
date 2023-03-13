namespace UniGame.LeoEcs.Debug.Editor
{
    public interface IEcsWorldSearchFilter
    {
        public EcsFilterData Execute(EcsFilterData filterData);
    }
}