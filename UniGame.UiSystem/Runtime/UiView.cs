namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using System;
    using Abstracts;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    public abstract class UiView<TModel> : 
        MonoBehaviour, IView
        where TModel : class, IViewModel
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        protected TModel context;
        protected BoolReactiveProperty visibility = new BoolReactiveProperty(false);
                
        #region public methods

        public IReadOnlyReactiveProperty<bool> IsActive => visibility;

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

            //bind model lifetime to local
            var modelLifeTime = model.LifeTime;
            modelLifeTime.AddTo(Close);

            //custom initialization
            OnInitialize(context,LifeTime);
            
            model.IsActive.
                Where(x => x).
                Subscribe(x => Show()).
                AddTo(lifeTimeDefinition.LifeTime);
            
            model.IsActive.
                Where(x => !x).
                Subscribe(x => Close()).
                AddTo(lifeTimeDefinition.LifeTime);
        }

        /// <summary>
        /// show active view
        /// </summary>
        public virtual void Show() {}

        /// <summary>
        /// hide view without release it
        /// </summary>
        public virtual void Hide() {}

        /// <summary>
        /// end of view lifetime
        /// </summary>
        public virtual void Close() => lifeTimeDefinition.Terminate();

        
        #endregion
        
        #region public properties

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        
        #endregion
        
        /// <summary>
        /// custom initialization methods
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lifeTime"></param>
        protected virtual void OnInitialize(TModel model, ILifeTime lifeTime) { }

        private void OnDestroy() => Close();
    }
}