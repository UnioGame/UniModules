using UniModule.UnityTools.UniPool.Scripts;
using UniNodeSystem;

namespace UniModule.UnityTools.Interfaces
{
    public interface IValueConnection<TValue>  : IDataValue<TValue>,IPoolable
    {
        
        string Id { get; }

        PortIO Direction { get; }
        
    }
}
