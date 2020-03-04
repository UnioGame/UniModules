namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public interface IUiManager : ILifeTimeContext
    {
        UniTask<T> Open<T>(IViewModel viewModel) 
            where T :Component, IView;
        
        UniTask<T> OpenWindow<T>(IViewModel viewModel) 
            where T :Component, IView;

        UniTask<T> OpenScreen<T>(IViewModel viewModel) 
            where T :Component, IView;
                
        bool CloseWindow<T>() where T :Component, IView;

        bool CloseScreen<T>() where T :Component, IView;


    }
}