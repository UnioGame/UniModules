using System;
using Assets.Tools.Utils;

namespace Assets.Scripts.MessageQueue {

    public interface IMessage : IPoolable {

        Type Type { get; }
        object Sender { get; }
        object Context { get; }

    }

}