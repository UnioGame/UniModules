using UniGreenModules.UniCore.Runtime.DataFlow;
using UniRx;
using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.Views
{
    using System;
    using Interfaces;

    public class ComponentViewModel<TModel> : MonoBehaviour , IViewModel<TModel>
    {
        private ViewModel<TModel> viewModel = new ViewModel<TModel>();

        public bool IsInitialized => viewModel.IsInitialized;

        public ILifeTime LifeTime => viewModel.LifeTime;

        public Type Type => viewModel.Type;

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

        protected virtual void OnDestroy() => Release();
    }
}
