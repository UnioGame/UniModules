using UniGreenModules.UniCore.Runtime.DataFlow;

namespace UniModules.UniGame.SerializableContext.Runtime.Systems
{
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces.Rx;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniStateMachine.Runtime.Interfaces;
    using UniRx;
    using UniRx.Async;

    public class ContextSystem : IAsyncState
    {
        #region private properties

        private LifeTimeDefinition _lifeTime;
        private IReadOnlyList<IObservableValue<IContext>> _contextContainers;

        #endregion

        public ContextSystem(IReadOnlyList<IObservableValue<IContext>> sources)
        {
            _lifeTime = new LifeTimeDefinition();
            _lifeTime.Release();
            _contextContainers = sources;
        }
        
        public ILifeTime LifeTime => _lifeTime;
        
        public bool IsActive => !_lifeTime.IsTerminated;

        public async UniTask<Unit> Execute()
        {
            if(IsActive) return Unit.Default;
            
            _lifeTime.Release();
            
            OnContainersLoaded(_contextContainers);
            
            return Unit.Default;
        }

        public async UniTask<Unit> Exit()
        {
            _lifeTime.Terminate();
            return Unit.Default;
        }

        #region private methods
        
        private void OnContainersLoaded(IReadOnlyList<IObservableValue<IContext>> containerAssets)
        {
            foreach (var containerAsset in containerAssets) {
                containerAsset.
                    Where(x=> x!=null).
                    Subscribe(OnContextAvailable).
                    AddTo(_lifeTime);
            }
        }

        protected virtual void OnContextAvailable(IContext context)
        {
            
        }

        
        #endregion

    }
}
