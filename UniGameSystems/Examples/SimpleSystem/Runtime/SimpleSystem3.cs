namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem
{
    using System.Collections;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniGameSystem.Runtime;
    using UniRoutine.Runtime;
    using UniRoutine.Runtime.Extension;

    public class SimpleSystem3 : GameService
    {
        
        protected override IContext OnBind(IContext context, ILifeTime lifeTime = null)
        {
            context.Publish(this);
            ReadyDelay(3).Execute();
            return context;
        }

        private IEnumerator ReadyDelay(float delay)
        {
            yield return this.WaitForSeconds(delay);
            isReady.Value = true;
        }
        
    }
}
