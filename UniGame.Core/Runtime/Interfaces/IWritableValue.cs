namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx;

    public interface IWritableValue
    {
        void CopyTo(IMessagePublisher target);
        
    }
}
