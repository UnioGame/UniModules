namespace UniModules.UniGame.RemoteData.SharedMessages
{
    using MessageData;

    public interface ISharedMessageProcessor
    {
        void ProcessMessage(AbstractSharedMessage message);
    }
}
