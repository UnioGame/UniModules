namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using Settings;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx.Async;
    using UnityEngine;

    public class GameUiViewManager : MonoBehaviour, 
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
            return await elementsController.Create<T>(viewModel,skinTag);
        }

        public async UniTask<T> OpenWindow<T>(IViewModel viewModel,string skinTag = "") where T : Component, IView
        {
            return await windowsController.Create<T>(viewModel,skinTag);
        }

        public async UniTask<T> OpenScreen<T>(IViewModel viewModel,string skinTag = "") where T : Component, IView
        {
            return await screensController.Create<T>(viewModel,skinTag);
        }

        public bool CloseWindow<T>() where T : Component, IView
        {
            return windowsController.Close<T>();
        }

        public bool CloseScreen<T>() where T : Component, IView
        {
            return screensController.Close<T>();
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
