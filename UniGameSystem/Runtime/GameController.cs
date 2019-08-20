using GBG.GameSystem.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.DataFlow;

namespace GBG.GameSystem.Runtime
{
    public class GameController : IGameController
    {
        protected LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        public void Dispose()
        {
            lifeTimeDefinition.Terminate();
        }

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
    }
}
