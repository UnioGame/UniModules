namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface IRecycleMessageBrocker : IMessageBroker, IPoolable
    {
    }
}