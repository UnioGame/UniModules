namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using DataFlow;
    using ObjectPool.Interfaces;

    public interface ILifeTimeCommand
    {
        void Execute(ILifeTime lifeTime);
    }
}