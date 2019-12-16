namespace UniGreenModules.UniGameSystem.Runtime
{
    using Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;

    public class GameService : IGameService
    {
        protected LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();

        public void Bind(IContext context)
        {
            
        }
        
        public void Dispose()
        {
            lifeTimeDefinition.Terminate();
        }

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
    }
}
