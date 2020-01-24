namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx;

    public interface IViewModel<TModel> : ITypeViewModel
    {
        /// <summary>
        /// data source
        /// </summary>
        IReadOnlyReactiveProperty<TModel> Model { get; }
        
        /// <summary>
        /// initialize view with source model
        /// </summary>
        /// <param name="model"></param>
        void Initialize(TModel model);
    }
}
