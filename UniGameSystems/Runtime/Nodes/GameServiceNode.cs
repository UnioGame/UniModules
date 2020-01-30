namespace UniGreenModules.UniGameSystem.Nodes
{
    using Runtime.Interfaces;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniNodes.Runtime.Nodes;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Base game service binder between Unity world and regular classes
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [CreateNodeMenuAttribute("GameSystem/Service Node")]
    public class GameServiceNode<TService> : 
        ContextNode
        where TService : IGameService, new()
    {
        private TService service = new TService();

        #region inspector

        [Header("Service Status")]
        [ReadOnlyValue]
        [SerializeField]
        private bool isReady;
        
        #endregion
        
        public bool waitForServiceReady = true;
        
        protected override void OnDataUpdated(IContext data, IContext source, IContext target)
        {
            GameLog.LogMessage($"{graph.name}:{name}: OnDataUpdated with {data.GetType().Name}");
            
            //bind service with context and node lifetime
            var context = service.Bind(data, LifeTime);

            service.IsReady.
                Subscribe(x => this.isReady = x).
                AddTo(LifeTime);
            
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
