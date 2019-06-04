namespace UniGreenModules.UniCore.Runtime.Views
{
    using DataFlow;
    using Rx.Extensions;
    using UniRx;

    public class ViewModel<TModel> : IViewModel<TModel>
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        private ReactiveProperty<TModel> contextModel = new ReactiveProperty<TModel>();
        
        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public IReadOnlyReactiveProperty<TModel> Model => contextModel;
        
        public void Initialize(TModel model)
        {
            Release();  
            contextModel = new ReactiveProperty<TModel>();
            contextModel.AddTo(LifeTime);
            OnInitialize(model);
        }

        public void Release()
        {
            lifeTimeDefinition.Release();
            OnRelease();
        }

        
        protected virtual void OnInitialize(TModel model) {}

        protected virtual void OnRelease(){}
    }
}
