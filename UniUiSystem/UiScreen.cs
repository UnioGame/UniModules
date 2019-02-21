using UnityEngine;

namespace UniUiSystem
{
    public class UiScreen : UiModule
    {
        #region inspector

        [SerializeField]
        protected Canvas _canvas;
        [SerializeField]
        protected CanvasGroup _canvasGroup;        

        #endregion      
        
        public Canvas Canvas => _canvas;

        public CanvasGroup CanvasGroup => _canvasGroup;

        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!_canvas)
                _canvas = GetComponent<Canvas>();
            if (!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();

        }
    }
}
