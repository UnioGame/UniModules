using UniGreenModules.UniCore.Runtime.DataFlow;

namespace UniGreenModules.UniNodeSystem.Nodes
{
    using Sirenix.OdinInspector;
    using UniCore.Runtime.ObjectPool;

    public class SyncGraphNode : UniGraphNode
    {
        [InlineEditor(InlineEditorModes.FullEditor)]
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
