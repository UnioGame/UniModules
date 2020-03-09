namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstracts;
    using Core.Runtime.Rx;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class ViewController : IViewController
    {
        private IViewResourceProvider resourceProvider;
        
        private List<IView> views = new List<IView>();

        private LifeTimeDefinition lifeTime = new LifeTimeDefinition();

        private RecycleReactiveProperty<Unit> visibilityChanged = new RecycleReactiveProperty<Unit>();
        
        #region constructor
        
        public ViewController(IViewResourceProvider viewResourceProvider)
        {
            resourceProvider = viewResourceProvider;
            visibilityChanged.
                ThrottleFrame(2).
                Subscribe(x => VisibilityStatusChanged()).
                AddTo(lifeTime.LifeTime);
        }
        
        #endregion
        
        #region public methods

        public void Dispose() => lifeTime.Terminate();

        public async UniTask<T> Create<T>(string skinTag = "") where T : Component, IView
        {
            var viewPromise = resourceProvider.
                LoadViewAsync<T>(skinTag);

            //start resource load, save disposable token
            var disposable = viewPromise.Subscribe();
            
            //wait until resource is load
            await UniTask.WaitUntil(() => viewPromise.IsReady.Value);

            var asset = viewPromise.Value.Value;
            
            //release promise
            viewPromise.Dispose();
            
            //if loading failed release resource immediately
            if (asset == null) {
                GameLog.LogError($"{nameof(ViewController)} View of Type {typeof(T).Name} not loaded");
                disposable.Dispose();
                return null;
            }

            var view = Add(asset);

            //bind disposable to View lifeTime
            var viewLifeTime = view.LifeTime;
            viewLifeTime.AddDispose(disposable);
            viewLifeTime.AddCleanUpAction(() => Close(view));

            //handle all view visibility changes
            view.IsActive.
                Subscribe(x => visibilityChanged.Value = (Unit.Default)).
                AddTo(viewLifeTime);

            return view;

        }

        public bool Hide<T>() where T : Component, IView
        {
            var view = Select<T>();
            view?.Hide();
            return view != null;
        }

        public void HideAll<T>() where T : Component, IView
        {
            foreach (var view in views) {
                if(view is T targetView)
                    targetView.Hide();
            }
        }
        
        public void HideAll()
        {
            foreach (var view in views) {
                view.Hide();
            }
        }

        public bool Close<T>() where T : Component, IView
        {
            var view = Select<T>();
            view?.Close();
            return view!=null;
        }

        public void CloseAll()
        {
            var buffer = ClassPool.Spawn<List<IView>>();
            buffer.AddRange(views);
            foreach (var view in buffer) {
                view.Close();
            }
            buffer.DespawnCollection();
        }

        #endregion

        /// <summary>
        /// proceed visibility changes on target view
        /// </summary>
        private void VisibilityStatusChanged()
        {
            OnVisibilityStatusChanged();
        }
        
        private void Close<T>(T view) where T : Component, IView
        {
            if (!view)
                return;
            
            //custom user action before cleanup view
            OnBeforeClose(view);
            //remove view Object
            if (views.Remove(view)) {
                //destroy view GameObject
                Object.Destroy(view.gameObject);
            }
        }
        
        private TView Select<TView>() where TView : Object, IView
        {
            return views.FirstOrDefault(x => typeof(TView) == x.GetType()) as TView;
        }

        /// <summary>
        /// Create View instance and place it in controller space
        /// </summary>
        /// <param name="asset">asset source</param>
        /// <param name="model">view model data</param>
        /// <typeparam name="TView"></typeparam>
        /// <returns>created view</returns>
        private TView Add<TView>(TView asset) where TView : Component, IView
        {
            //create instance of view
            var view = Object.
                Instantiate(asset.gameObject).
                GetComponent<TView>();
            
            //add view to loaded view items
            views.Add(view);

            //custom view method call
            OnViewOpen(view);
            
            return view;
        }
        
        protected virtual void OnBeforeClose<T>(T view) where T : Component, IView {}

        protected virtual void OnViewOpen<T>(T view) where T : Component, IView {}

        protected virtual void OnVisibilityStatusChanged() { }
    }
}
