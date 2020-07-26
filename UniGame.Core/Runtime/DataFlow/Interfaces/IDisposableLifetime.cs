namespace UniGreenModules.UniGame.Core.Runtime.Common
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IDisposableLifetime : 
        IDisposableItem, 
        ILifeTimeContext, 
        ILifeTime
    {
    }
}