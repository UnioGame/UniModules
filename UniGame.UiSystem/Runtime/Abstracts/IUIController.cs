namespace Taktika.UI.Abstracts
{
    using System;
    using MVVM.Abstracts;
    using UniRx.Async;

    public interface IUIController : IDisposable
    {
        UniTask<T> Open<T>(IViewModel viewModel) 
            where T :class, IView;
    }
}