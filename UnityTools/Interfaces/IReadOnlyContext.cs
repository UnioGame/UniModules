using UniModule.UnityTools.DataFlow;

namespace UniModule.UnityTools.Interfaces
{
    public interface IReadOnlyContext
    {
        /// <summary>
        /// lifetime of this context
        /// </summary>
        ILifeTime LifeTime { get; }

        /// <summary>
        /// get data from context object
        /// </summary>
        TData Get<TData>();
    }
}