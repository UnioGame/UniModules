namespace UniModule.UnityTools.Common
{
    public interface ITypeDataContainer
    {
        TData Get<TData>();
        bool Remove<TData>();
        void Add<TData>(TData data);

        bool HasData<TData>();
    }
}