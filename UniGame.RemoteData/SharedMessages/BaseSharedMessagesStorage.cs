namespace UniModules.UniGame.RemoteData.SharedMessages {
    using System;
    using System.Threading.Tasks;
    using MessageData;

    public abstract class BaseSharedMessagesStorage : IDisposable
    {
        public abstract void StartListen();

        public abstract Task CommitMessage(string userId, AbstractSharedMessage message);

        public abstract void Dispose();

        public abstract event Action<AbstractSharedMessage> MessageAdded;
    }
}
