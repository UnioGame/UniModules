using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface IWritableValue
    {
        void CopyTo(IMessagePublisher target);
        
    }
}
