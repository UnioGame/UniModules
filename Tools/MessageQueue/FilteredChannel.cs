using Assets.Scripts.MessageQueue;


public class FilteredChannel<TMessage> : BaseChannel
    where TMessage : class
{

    protected override bool ValidateOutputMessage(IMessage message) {

        return message != null && message.Context is TMessage;

    }

    protected override bool ValidateInputMessage(IMessage message) {

        return message != null && message.Context is TMessage;

    }
    

}
