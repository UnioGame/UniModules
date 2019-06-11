namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Interfaces;
    using UniRx;

    public interface IRecycleMessageBrocker : IMessageBroker, IPoolable
    {
    }
}