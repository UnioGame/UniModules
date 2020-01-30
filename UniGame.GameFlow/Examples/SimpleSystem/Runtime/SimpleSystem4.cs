namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameSystem.Runtime;
    using UniRx;

    public class SimpleSystem4 : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime)
        {
            isReady.Value = false;
            context.Publish(this);
            
            context.Receive<SimpleSystem3>().
                Where(x => x != null).
                Do(x => isReady.Value = true).
                Subscribe().
                AddTo(lifeTime);
            
            return context;
        }

    }
}
