namespace UniModules.UniGame.SerializableContext.Runtime.Systems
{
    using UniModules.UniGame.Core.Runtime.ScriptableObjects;
    using System.Collections.Generic;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Addressables;
    using UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes;
    using UniGreenModules.UniStateMachine.Runtime.Interfaces;
    using UniRx;
    using UniRx.Async;

    public class ContextSystemAsset : LifetimeScriptableObject, IAsyncState
    {
        #region inspector
        
        /// <summary>
        /// system data sources
        /// </summary>
        public List<ContextContainerAssetReference> contextSources = 
            new List<ContextContainerAssetReference>();

        #endregion
        
        #region private properties

        private LifeTimeDefinition _executionLifetime;
        private List<ContextContainerAsset> _contextContainers;

        #endregion

        public bool IsActive => !_executionLifetime.IsTerminated;

        public async UniTask<Unit> Execute()
        {
            if(IsActive) return Unit.Default;
            
            _executionLifetime.Release();
            _contextContainers = await InitializeSources();
            
            OnContainersLoaded(_contextContainers);
            
            return Unit.Default;
        }

        public async UniTask<Unit> Exit()
        {
            _executionLifetime.Terminate();
            return Unit.Default;
        }

        #region private methods

        private async UniTask<List<ContextContainerAsset>> InitializeSources()
        {
            var container = new List<ContextContainerAsset>();
            if (contextSources?.Count <= 0) {
                GameLog.LogRuntime($"EMPTY context system sources {name}");
                return container;
            }

            await contextSources.LoadAssetsTaskAsync(container, _executionLifetime);

            return container;
        }

        private void OnContainersLoaded(IReadOnlyList<ContextContainerAsset> containerAssets)
        {
            foreach (var containerAsset in containerAssets) {
                containerAsset.
                    Where(x=> x!=null).
                    Subscribe(UpdateContext).
                    AddTo(_executionLifetime);
            }
        }

        protected virtual void UpdateContext(IContext context)
        {
            
        }
        
        protected sealed override void OnActivate()
        {
            _executionLifetime = new LifeTimeDefinition();
            _executionLifetime.Terminate();
            LifeTime.AddCleanUpAction(_executionLifetime.Terminate);
        }
        
        #endregion
    }
}
