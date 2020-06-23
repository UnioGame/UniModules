namespace UniModules.UniGame.RemoteData.RemoteData
{
    using System;

    public abstract class RemoteObjectsProvider : IDisposable
    {
        public abstract void Dispose();

        public abstract RemoteObjectHandler<T> GetRemoteObject<T>(string path);

        public abstract string GetIdForNewObject(string path);
    }
}
