using UniRx;
using UnityTools.Common;

namespace UnityTools.Interfaces
{
    public interface IWritableValue
    {
        void CopyTo(IMessagePublisher target);
        
    }
}
