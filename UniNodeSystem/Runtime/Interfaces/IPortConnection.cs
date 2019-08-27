namespace UniGreenModules.UniNodeSystem.Runtime.Connections
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;

    public interface IPortConnection : IContextWriter, IConnector<ITypeData>, IPoolable
    {
    }
}