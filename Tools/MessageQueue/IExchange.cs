namespace Assets.Scripts.MessageQueue
{
    public interface IExchange
    {
        void Send(Message message);
    }
}
