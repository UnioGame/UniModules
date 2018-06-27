using Assets.Tools.Utils;

namespace Assets.Tools.MessageQueue
{

    public abstract class MessageData: IPoolable {

        public abstract void Release();

    }

}
