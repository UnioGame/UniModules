using UnityEngine;

namespace Taktika.UI
{
    using System;
    using MVVM.Abstracts;
    using UniGreenModules.UniCore.Runtime.Common;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;

    public abstract class UiView<TModel> : 
        MonoBehaviour, IView
        where TModel : class, IViewModel
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        protected TModel context;
        
        public void SetViewModel(IViewModel model)
        {
            //restart view lifetime
            lifeTimeDefinition.Release();
            
            //save model as context data
            if (model is TModel viewModel) {
                context = viewModel;
            }
            else {
                throw  new ArgumentException($"VIEW: {name} wrong model type. Target type {typeof(TModel).Name} : model Type {model?.GetType().Name}");
            }
            
            //custom initialization
            OnInitialize(context,LifeTime);

            var terminateAction = ClassPool.Spawn<DisposableAction>();
            
            //bind local lifetime to 
            terminateAction.Initialize(lifeTimeDefinition.Terminate);
            var lifeTime = model.LifeTime;
            
        }
        
        public abstract void Open();

        public void Release() => lifeTimeDefinition.Terminate();
        
        #region public properties

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        
        #endregion
        
        protected virtual void OnInitialize(TModel model, ILifeTime lifeTime) { }

        private void OnDestroy() => lifeTimeDefinition.Terminate();
    }
}