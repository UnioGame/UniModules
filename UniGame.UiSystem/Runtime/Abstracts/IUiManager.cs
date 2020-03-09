namespace UniGreenModules.UniGame.UiSystem.Runtime
{
    using Abstracts;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public interface IUiManager : ILifeTimeContext, IViewElementFactory
    {
        bool CloseWindow<T>() where T :Component, IView;

        bool CloseScreen<T>() where T :Component, IView;
    }
}