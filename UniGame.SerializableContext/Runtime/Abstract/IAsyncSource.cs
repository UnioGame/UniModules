namespace UniModules.UniGame.SerializableContext.Runtime.Abstract
{
    using Core.Runtime.Interfaces;
    using UniGreenModules.UniContextData.Runtime.Interfaces;

    public interface IAsyncSource : IAsyncContextDataSource, ILifeTimeContext
    {
    }
}