namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using DataFlow;
    using DataFlow.Interfaces;

    public interface ILifeTimeCommand
    {
        void Execute(ILifeTime lifeTime);
    }
}