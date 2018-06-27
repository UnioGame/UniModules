using System;

namespace Assets.Scripts.MessageQueue
{
    public interface IMessageSystem : IDisposable
    {
        /// <summary>
        /// add source channel
        /// </summary>
        void AddSource(IChannel channel);

        /// <summary>
        /// remove channel source
        /// </summary>
        void RemoveSource(IChannel channel);

        ///  <summary>
        ///  register channel to target messages
        ///  </summary>
        /// <param name="channel">message channel</param>
        void Register(IChannel channel);

        /// <summary>
        /// remove subscription
        /// </summary>
        /// <param name="channel">target channel</param>
        void RemoveRegistration(IChannel channel);


    }
}