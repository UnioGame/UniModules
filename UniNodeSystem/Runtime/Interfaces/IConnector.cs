using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public interface IConnector<T>
    {
        IConnector<T> Connect(T connection);
        void Disconnect(T connection);
    }
}