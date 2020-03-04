namespace UniGreenModules.UniGame.UiSystem.Runtime.Abstracts
{
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IView : ILifeTimeContext
    {
        IReadOnlyReactiveProperty<bool> IsActive { get; }

        void SetViewModel(IViewModel vm);
        
        void Close();

        void Show();

        void Hide();

    }
}