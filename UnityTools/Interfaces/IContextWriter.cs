namespace UniModule.UnityTools.Common
{
    public interface IContextWriter
    {
        bool Remove<TData>();

        void RemoveAll();

        void Add<TData>(TData data);
    }
}