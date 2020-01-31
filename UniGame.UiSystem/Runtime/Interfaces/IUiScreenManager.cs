namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    public interface IUiScreenManager
    {

        /// <summary>
        /// is screen already shown
        /// </summary>
        /// <typeparam name="TModel">target screen model</typeparam>
        bool IsShown<TModel>();


        /// <summary>
        /// return target screen by model type
        /// </summary>
        /// <param name="useShared">if set to TRUE, then can return already active screen</param>
        /// <typeparam name="TModel">target ui model</typeparam>
        /// <returns>screen result</returns>
        IUiScreen<TModel> GetScreen<TModel>(bool useShared = true);

        
        
    }
}
