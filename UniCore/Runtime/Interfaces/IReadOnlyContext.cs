namespace UniModule.UnityTools.Interfaces
{
    public interface IReadOnlyContext
    {
        /// <summary>
        /// get data from context object
        /// </summary>
        TData Get<TData>();
        
        /// <summary>
        /// is context contain target data
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns>is content found</returns>
        bool Contains<TData>();

    }
}