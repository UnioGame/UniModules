namespace UniGreenModules.UniGameSystem.Nodes
{
    using Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniNodes.Runtime.Nodes;
    using UniRx;

    /// <summary>
    /// Base game service binder between Unity world and regular classes
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [CreateNodeMenuAttribute("Services/Service Node")]
    public class GameServiceNode<TService> : 
        ContextNode
        where TService : IGameService, new()
    {
        private TService service = new TService();

        public bool waitForServiceReady = true;
        
        protected override void OnDataUpdated(IContext data, IContext source, IContext target)
        {
            //bind service with context and node lifetime
            var context = service.Bind(data, LifeTime);
            
            //is await options is active?
            if (waitForServiceReady) {
                service.IsReady.
                    Where(x => x).
                    Do(x => base.OnDataUpdated(context, source, target)).
                    Subscribe().
                    AddTo(LifeTime);
            }
            else {
                base.OnDataUpdated(context, source, target);
            }

        }
    }
}
