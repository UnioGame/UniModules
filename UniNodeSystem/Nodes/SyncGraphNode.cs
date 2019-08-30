using UniGreenModules.UniCore.Runtime.DataFlow;

namespace UniGreenModules.UniNodeSystem.Nodes
{
    using UniCore.Runtime.ObjectPool;

    public class SyncGraphNode : UniGraphNode
    {
        public UniGraph graphAsset;

        private UniGraph graphInstance;
        
        public override UniGraph LoadOrigin() => graphAsset;

        protected override UniGraph CreateGraph(ILifeTime lifeTime)
        {
            if (graphInstance) return graphInstance;
            graphInstance = graphAsset.Spawn();

            lifeTime.AddCleanUpAction(() => graphInstance?.Despawn());
            return graphAsset;
        }
    }
}
