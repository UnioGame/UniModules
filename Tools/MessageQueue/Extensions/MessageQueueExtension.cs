using System;
using Assets.Tools.Utils;
using Assets.Scripts.MessageQueue;
using UnityEngine;

public static class MessageQueueExtension {

    public static Message CreateMessage(this object data, Type type = null, object sender = null, bool despawn = true) {

        var message = LeanExtension.Spawn<Message>();

        var messageType = type == null ?  message.GetType() : type;  
        
        message.MessageType = messageType;
        message.SenderObject = sender;
        message.MessageData = data;
        message.Despawn = despawn;

        return message;
    }
    
    public static void Post<T>(this IChannel channel,Action<T> configurationAction = null) where T : class , new() {

        var messageType = typeof(T);

        if (channel == null) {
            Debug.LogError("Trying to send message with NULL channel");
            return;
        }

        var messageData = LeanExtension.Spawn<T>();

        if (messageData == null) {
            Debug.LogError("Message data of Channel in NULL");
            return;
        }

        configurationAction?.Invoke(messageData);

        var message = messageData.CreateMessage(messageType,channel);
        channel.Send(message);

    }

}
