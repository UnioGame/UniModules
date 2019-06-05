namespace UniGreenModules.GBG.UiManager.Runtime
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public interface IUiView<TModel> : IViewModel<TModel>
    {
        
        bool IsActive { get; }

        RectTransform RectTransform { get; }

        void UpdateView();
        
        void SetState(bool active);
        
    }
}