namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using ObjectPool.Interfaces;
    using UniRx;

    public interface IRecycleMessageBrocker : IMessageBroker, IPoolable
    {
    }
}