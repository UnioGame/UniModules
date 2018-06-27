using System;
using Assets.Scripts.MessageQueue;
using Assets.Tools.MessageQueue;

public interface IFilteredChannel : IChannel {

    void RegisterAction<T>(Action<T> callback)
        where T : MessageData;
    
}