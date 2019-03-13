using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public interface IConnector<T>
    {
        void Connect(T connection);
        void Remove(T connection);
    }
}