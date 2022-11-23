#pragma warning disable 1998

namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine
{
    using Cysharp.Threading.Tasks;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using global::UniGame.Core.Runtime;
    using UniRx;
    

    public class AsyncStateBehaviour : IStateBehaviour<UniTask>
    {
        protected readonly BoolReactiveProperty isActive = new BoolReactiveProperty(false);
        protected readonly BoolReactiveProperty isInitialized = new BoolReactiveProperty(false);
        protected readonly LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        public IReadOnlyReactiveProperty<bool> IsActive => isActive;

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public async UniTask Execute()
        {

            if (isActive.Value)
                await OnAlreadyActive();
            
            lifeTimeDefinition.Release();

            
            LifeTime.AddCleanUpAction(OnExit);
            LifeTime.AddCleanUpAction(() => isActive.Value = false);
            LifeTime.AddCleanUpAction(() => isInitialized.Value = false);
            
            isActive.Value = true;
            
            await OnInitialize();

            await OnExecute();

            await OnPostExecute();
            
        }

        public void Exit() {
            
            lifeTimeDefinition.Terminate();
            
        }

        protected virtual void OnExit(){}

        protected virtual async UniTask OnInitialize()
        {
        }

        protected virtual async UniTask OnExecute()
        {
            
        }

        protected virtual async UniTask OnPostExecute()
        {
            
        }

        protected virtual async UniTask OnAlreadyActive()
        {
            while (isActive.Value)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

    }
}
