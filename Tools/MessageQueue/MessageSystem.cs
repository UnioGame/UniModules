using System;
using System.Diagnostics;
using Assets.Tools.Utils;
using Assets.Scripts.ProfilerTools;
using UnityEngine;

namespace Assets.Scripts.MessageQueue
{
    public class MessageSystem : ObserableRxHub<IMessage>
    {

        private static Color _messageLogColor = new Color(0, 100, 0);

        #region private methods

        protected override void OnMessageReceived(IMessage message, IObservable<IMessage> sender)
        {
            LogMessage(message,true);

            if (message == null || message.Type == null) {
                return;
            }

            foreach (var output in _subscribers) {

                var observer = output.Key;
                if (observer == sender) continue;

                observer.OnNext(message);
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

        #endregion


    }
}
