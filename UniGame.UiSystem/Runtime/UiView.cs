namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using System;
    using Abstracts;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    public abstract class UiView<TViewModel> : 
        MonoBehaviour, IView
        where TViewModel : class, IViewModel
    {
        
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        /// <summary>
        /// view model context
        /// </summary>
        protected TViewModel context;
        
        /// <summary>
        /// ui element visibility status
        /// </summary>
        protected BoolReactiveProperty visibility = new BoolReactiveProperty(false);
          
        #region public properties

        public IReadOnlyReactiveProperty<bool> IsActive => visibility;

        
        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;


        #endregion
        
        #region public methods

        public void SetViewModel(IViewModel model)
        {
            //restart view lifetime
            lifeTimeDefinition.Release();
            
            //save model as context data
            if (model is TViewModel viewModel) {
                context = viewModel;
            }
            else {
                throw  new ArgumentException($"VIEW: {name} wrong model type. Target type {typeof(TViewModel).Name} : model Type {model?.GetType().Name}");
            }

            //bind model lifetime to local
            var modelLifeTime = model.LifeTime;
            //terminate if model lifetime ended
            lifeTimeDefinition.AddTo(modelLifeTime);
            //terminate model when view closed
            LifeTime.AddDispose(model);

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
        public virtual void Show() => visibility.Value = true;

        /// <summary>
        /// hide view without release it
        /// </summary>
        public virtual void Hide() => visibility.Value = false;

        /// <summary>
        /// end of view lifetime
        /// </summary>
        public void Close() => lifeTimeDefinition.Terminate();

        
        #endregion

        /// <summary>
        /// custom initialization methods
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lifeTime"></param>
        protected virtual void OnInitialize(TViewModel model, ILifeTime lifeTime) { }

        private void OnDestroy() => Close();

        
        
    }
}