using System;

namespace Assets.Scripts.MessageQueue {
    public interface IInputChannel : IDisposable {
        bool IsDisposed { get; }
        void Send(IMessage message);
    }
}