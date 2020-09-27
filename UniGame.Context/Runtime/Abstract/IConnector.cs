namespace UniGame.UniNodes.NodeSystem.Runtime.Interfaces
{
    using System;

    public interface IConnector<T>
    {
        /// <summary>
        /// retranslate all data of connecter to target connection
        /// </summary>
        /// <param name="connection">target output channel</param>
        /// <returns>disposable connection token</returns>
        IDisposable Bind(T connection);
        
    }
}