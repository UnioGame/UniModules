namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IContextWriter
    {
        bool Remove<TData>();

        void CleanUp();

        void Add<TData>(TData data);
    }
}