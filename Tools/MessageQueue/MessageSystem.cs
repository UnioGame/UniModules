using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Tools.Utils;
using Assets.Scripts.ProfilerTools;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.MessageQueue
{
    public class MessageSystem : IMessageSystem
    {

        private static Color _messageLogColor = new Color(0, 100, 0);


        #region private properties

        /// <summary>
        /// added channels
        /// </summary>
        private readonly Dictionary<IChannel, IDisposable> _channels = new Dictionary<IChannel, IDisposable>();
        private readonly List<IChannel> _outputChannels = new List<IChannel>();
        private readonly Dictionary<IChannel,int> _outputChannelsMap = new Dictionary<IChannel, int>();

        #endregion

        #region constructor

        public MessageSystem() { }

        #endregion

        public void Register(IChannel channel) {
            int index;
            if (_outputChannelsMap.TryGetValue(channel, out index) == false) {
                index = _outputChannels.Count;
                _outputChannels.Add(channel);
                _outputChannelsMap[channel] = index;
            }
        }

        public void RemoveRegistration(IChannel channel)
        {
            Unregister(channel);
        }

        public void AddSource(IChannel channel)
        {
            if (_channels.ContainsKey(channel))
            {
                GameLog.LogError("Channel already registered");
                return;
            }
            var disposable = channel.Output.Subscribe(x => OnMessageReceived(x, channel));
            _channels[channel] = disposable;
        }

        public void RemoveSource(IChannel channel)
        {
            if (!_channels.ContainsKey(channel))
                return;
            _channels[channel].Dispose();
            _channels.Remove(channel);
        }

        #region private methods

        private void OnMessageReceived(IMessage message, IChannel sender)
        {
            LogMessage(message,true);

            if (message == null || message.Type == null)
            {
                return;
            }

            foreach (var channel in _outputChannels) {

                if (channel.IsDisposed)
                {
                    Unregister(channel);
                    continue;
                }

                if (channel == sender)continue;
                
                if (!channel.IsRegistered(message.Type))continue;

                channel.Send(message);
            }
            //relese message Data
            message.Despawn();
        }


        [Conditional("MSQ_LOGS_ENABLED")]
        private void LogMessage(IMessage message,bool received)
        {
            if (message == null) {
                GameLog.Log($"MessageBus: Received NULL message {message}");
                return;
            }

            if (received) {
                var type = message.Type == null ? string.Empty : message.Type.FullName;
                GameLog.LogFormat("[MSQ Received] Type:{0} Content: {1} Sender {2}",
                    _messageLogColor, type, message.Context, message.Sender);
            }
        }

        private void Unregister(IChannel channel) {

            int index;
            if (_outputChannelsMap.TryGetValue(channel, out index)) {
                _outputChannels.Remove(channel);
                _outputChannelsMap.Remove(channel);
            }

        }

        public void Dispose()
        {
            foreach (var disposable in _channels) {
                disposable.Value.Dispose();
            }
            _channels.Clear();
            _outputChannels.Clear();
        }

        #endregion


    }
}
