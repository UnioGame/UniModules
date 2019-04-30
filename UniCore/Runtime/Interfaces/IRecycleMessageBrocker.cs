namespace UniModule.UnityTools.RecycleRx
{
    using UniPool.Scripts;
    using UniRx;

    public interface IRecycleMessageBrocker : IMessageBroker, IPoolable
    {
    }
}