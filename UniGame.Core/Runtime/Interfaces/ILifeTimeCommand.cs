namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using DataFlow;
    using DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public interface ILifeTimeCommand
    {
        void Execute(ILifeTime lifeTime);
    }
}