using System;
using Assets.Tools.Utils;

namespace Assets.Scripts.MessageQueue
{
    public class Message : IResetable, IMessage {

        public Type MessageType;
        public object SenderObject;
        public object MessageData;
        public bool Despawn;

        public Type Type => MessageType;

        public object Sender => SenderObject;

        public object Context => MessageData;

        /// <summary>
        /// release message refs
        /// </summary>
        public virtual void Reset() {

            MessageType = null;
            SenderObject = null;
            if(Despawn)
                MessageData.Despawn();
            MessageData = null;
        }

        public void Release() {
            Reset();
        }
    }
}
