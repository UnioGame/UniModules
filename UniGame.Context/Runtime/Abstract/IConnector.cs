namespace UniGame.UniNodes.NodeSystem.Runtime.Interfaces
{
    using System;

    public interface IConnector<T>
    {
        IDisposable Bind(T connection);
        
    }
}