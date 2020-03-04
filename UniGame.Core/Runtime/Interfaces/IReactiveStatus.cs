namespace UniGame.UniNodes.GameFlow.Runtime.Interfaces
{
    using UniRx;

    public interface IReactiveStatus
    {
        /// <summary>
        /// is service ready to work
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsReady { get; }
    }
}