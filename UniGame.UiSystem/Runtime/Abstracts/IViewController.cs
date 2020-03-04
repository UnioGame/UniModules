namespace UniGreenModules.UniGame.UiSystem.Runtime.Abstracts
{
    using System;
    using UniRx.Async;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public interface IViewController : IDisposable
    {
        UniTask<T> Open<T>(IViewModel viewModel) 
            where T :Component, IView;

        bool Hide<T>() where T :Component, IView;

        void HideAll();

        void HideAll<T>() where T : Component, IView;
        
        bool Close<T>() where T :Component, IView;
        
        void CloseAll();

    }
}