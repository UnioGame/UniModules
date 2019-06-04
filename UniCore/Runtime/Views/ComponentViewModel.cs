using UniGreenModules.UniCore.Runtime.DataFlow;
using UniRx;
using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.Views
{
    public class ComponentViewModel<TModel> : MonoBehaviour , IViewModel<TModel>
    {
        private ViewModel<TModel> viewModel = new ViewModel<TModel>();

        public ILifeTime LifeTime => viewModel.LifeTime;

        public IReadOnlyReactiveProperty<TModel> Model => viewModel.Model;
    
        public void Initialize(TModel model)
        {
            viewModel.Initialize(model);
            OnInitialize(model);
        }
        
        public void Release()
        {
            viewModel.Release();
            OnRelease();
        }


        protected virtual void OnInitialize(TModel model){}

        protected virtual void OnRelease(){}
    }
}
