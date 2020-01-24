namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces
{
    public interface IPoolContainer
    {
        bool Contains<T>() where T : class;
        
        T Pop<T>() where T : class;

        void Push<T>(T item) where T : class;
    }
}