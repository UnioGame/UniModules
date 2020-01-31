namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public interface IUiView<TModel> : IViewModel<TModel>
    {
        RectTransform RectTransform { get; }

        /// <summary>
        /// view - update trigger
        /// </summary>
        void UpdateView();
        
    }
}