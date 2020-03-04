namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using UniRx.Async;

    public interface IUiManager
    {
        UniTask<T> Open<T>(IViewModel viewModel) 
            where T :class, IView;
        
        UniTask<T> OpenWindow<T>(IViewModel viewModel) 
            where T :class, IView;

        UniTask<T> OpenScreen<T>(IViewModel viewModel) 
            where T :class, IView;
                
        UniTask<T> CloseWindow<T>() where T :class, IView;

        UniTask<T> CloseScreen<T>() where T :class, IView;


    }
}