namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IAsyncSource : IAsyncContextDataSource, ILifeTimeContext
    {
    }
}