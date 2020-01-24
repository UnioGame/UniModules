namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx;

    public interface IContextWriter : IMessagePublisher
    {
        
        bool Remove<TData>();

        void CleanUp();

    }
}