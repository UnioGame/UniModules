namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using System;
    using Abstracts;
    using Settings;
    using Sirenix.OdinInspector;
    using UniRx.Async;
    using UnityEngine;

    public class GameUiViewManager : MonoBehaviour, IUiManager
    {
        #region inspector data
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
        [InlineEditor]
#endif
        public UiViewSourceSettings settings;
        
        public Canvas screenCanvas;

        public Canvas windowsCanvas;
        
        #endregion
        
        #region private fields

        private ViewController windowsController;
        private ViewController screensController;
        private ViewController elementsController;
        
        #endregion

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
        
        public UniTask<T> Open<T>(IViewModel viewModel) where T : class, IView => throw new System.NotImplementedException();

        public UniTask<T> OpenWindow<T>(IViewModel viewModel) where T : class, IView => throw new System.NotImplementedException();

        public UniTask<T> OpenScreen<T>(IViewModel viewModel) where T : class, IView => throw new System.NotImplementedException();

        public UniTask<T> CloseWindow<T>() where T : class, IView => throw new System.NotImplementedException();

        public UniTask<T> CloseScreen<T>() where T : class, IView => throw new System.NotImplementedException();

        
        private void Awake()
        {
               
        }
    }
}
