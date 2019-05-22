namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IContextWriter
    {
        bool Remove<TData>();

        void RemoveAll();

        void Add<TData>(TData data);
    }
}