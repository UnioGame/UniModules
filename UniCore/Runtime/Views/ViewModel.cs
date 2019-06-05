namespace UniGreenModules.UniCore.Runtime.Views
{
    using System;
    using DataFlow;
    using Interfaces;
    using Rx.Extensions;
    using UniRx;

    public class ViewModel<TModel> : IViewModel<TModel>
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        private ReactiveProperty<TModel> contextModel = new ReactiveProperty<TModel>();

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public Type Type { get; } = typeof(TModel);

        public IReadOnlyReactiveProperty<TModel> Model => contextModel;

        public bool IsInitialized => LifeTime.IsTerminated;

        public void Initialize(TModel model)
        {
            Release();  
            //restart lifetime
            lifeTimeDefinition.Release();
            
            contextModel = new ReactiveProperty<TModel>();
            contextModel.AddTo(LifeTime);
            OnInitialize(model);
        }

        public void Release()
        {
            lifeTimeDefinition.Terminate();
            OnRelease();
        }

        
        protected virtual void OnInitialize(TModel model) {}

        protected virtual void OnRelease(){}
    }
}
