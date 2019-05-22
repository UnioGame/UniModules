using UniNodeSystem;

namespace UniModule.UnityTools.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public interface IValueConnection<TValue>  : IDataValue<TValue>,IPoolable
    {
        
        string Id { get; }

        PortIO Direction { get; }
        
    }
}
