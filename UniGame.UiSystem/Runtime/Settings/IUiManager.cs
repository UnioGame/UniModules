namespace UniGreenModules.UniGame.UiSystem.Runtime.Settings
{
    using System;
    using Abstracts;

    public interface IUiManager : IDisposable
    {
        IViewResourceProvider UIResourceProvider { get; }
    }
}