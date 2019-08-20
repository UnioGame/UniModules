namespace GBG.GameSystem.Runtime.Interfaces
{
    using UniGreenModules.UniCore.Runtime.DataFlow;

    public interface IBinder
    {

        ILifeTime Bind(ILifeTime lifeTime);

    }
}
