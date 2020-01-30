namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniGameSystem.Runtime;

    public class SimpleSystem2 : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime = null)
        {
            context.Publish(this);
            isReady.Value = true;
            return context;
        }    
    }
}
