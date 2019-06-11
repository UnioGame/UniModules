namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    public interface IConnector<T>
    {
        IConnector<T> Connect(T connection);
        void Disconnect(T connection);
    }
}