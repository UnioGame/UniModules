using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public interface IConnector<T>
    {
        T Connect(T connection);
        void Disconnect(T connection);
    }
}