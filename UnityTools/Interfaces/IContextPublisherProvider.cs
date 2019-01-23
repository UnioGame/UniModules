using UniRx;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IContextPublisherProvider<TContext>
    {
    
        IMessagePublisher GetPublisher(TContext context);

    }
}