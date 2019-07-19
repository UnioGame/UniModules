using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx.Async;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    using Castle.Components.DictionaryAdapter.Xml;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniRx;

    public class AsyncStateBehaviour : IStateBehaviour<UniTask>
    {
        protected readonly BoolReactiveProperty isActive = new BoolReactiveProperty(false);
        protected readonly BoolReactiveProperty isInitialized = new BoolReactiveProperty(false);
        protected readonly LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        public IReadOnlyReactiveProperty<bool> IsActive => isActive;

        public IReadOnlyReactiveProperty<bool> IsInitialized => isInitialized;

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
            
            isInitialized.Value = await OnInitialize();

            await OnExecute();

            await OnPostExecute();
            
        }

        public void Exit() {
            
            lifeTimeDefinition.Terminate();
            
        }

        protected virtual void OnExit(){}

        protected virtual async UniTask<bool> OnInitialize()
        {
            return true;
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
