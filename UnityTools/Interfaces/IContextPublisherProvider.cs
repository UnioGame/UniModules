using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContextPublisherProvider<TContext>
    {
    
        IMessagePublisher GetPublisher(TContext context);

    }
}