namespace UniGreenModules.UniGame.Core.Runtime.Common
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;

    public interface IDisposableLifetime : IDisposableItem, ILifeTimeContext, ILifeTime
    {
    }
}