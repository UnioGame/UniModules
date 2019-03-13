using UniModule.UnityTools.DataFlow;

namespace UniModule.UnityTools.Interfaces
{
    public interface IReadOnlyContext
    {
        /// <summary>
        /// get data from context object
        /// </summary>
        TData Get<TData>();
    }
}