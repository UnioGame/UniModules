namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using Settings;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class GameViewSystem : MonoBehaviour, 
        IUiManager
    {
        #region inspector data
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.InlineEditor]
#endif
        public UiViewSourceSettings settings;
        
        public Canvas screenCanvas;

        public Canvas windowsCanvas;
        
        #endregion
        
        #region private fields

        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        private CanvasViewController windowsController;
        private CanvasViewController screensController;
        private ViewController elementsController;
        
        #endregion

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;
        
        public void Dispose() => lifeTimeDefinition.Terminate();

        public async UniTask<T> Open<T>(IViewModel viewModel,string skinTag = "") where T : Component, IView
        {
            return InitializeView(
                await elementsController.Create<T>(skinTag),
                viewModel);
        }

        public async UniTask<T> OpenWindow<T>(IViewModel viewModel,string skinTag = "") where T : Component, IView
        {
            return InitializeView(
                await windowsController.Create<T>(skinTag),
                viewModel);
        }

        public async UniTask<T> OpenScreen<T>(IViewModel viewModel,string skinTag = "") where T : Component, IView
        {
            return InitializeView(
                await screensController.Create<T>(skinTag),
                viewModel);
        }

        public bool CloseWindow<T>() where T : Component, IView
        {
            return windowsController.Close<T>();
        }

        public bool CloseScreen<T>() where T : Component, IView
        {
            return screensController.Close<T>();
        }

                
        private void Close<T>(T view) where T : Component, IView
        {
            //custom user action before cleanup view
            OnBeforeClose(view);
            //remove view Object
            views.Remove(view);
            //destroy view GameObject
            Object.Destroy(view.gameObject);
        }

        private T InitializeView<T>(T view, IViewModel viewModel)
            where T : Component, IView
        {
                        
            //initialize view with model data
            view.Initialize(viewModel,this);
            
            //update view active state by base view model data
            viewModel.IsActive.
                Where(x => x).
                Subscribe(x => view.Show()).
                AddTo(view.LifeTime);
            
            viewModel.IsActive.
                Where(x => !x).
                Subscribe(x => view.Close()).
                AddTo(view.LifeTime);

            return view;
        }
        
        private void Start()
        {
            settings.Initialize();
            var resourceProvider = settings.UIResourceProvider;
            
            windowsController = new CanvasViewController(windowsCanvas,resourceProvider).AddTo(LifeTime);
            screensController = new CanvasViewController(screenCanvas,resourceProvider).AddTo(LifeTime);
            elementsController = new ViewController(resourceProvider).AddTo(LifeTime);
        }

    }
}
