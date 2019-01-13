using UnityTools.Common;

namespace UnityTools.Interfaces
{
    public interface IWritableValue
    {
        void CopyTo(Common.IDataWriter target);
        
    }
}
