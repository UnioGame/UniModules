namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IContextProvider
    {

        TData GetContext<TData>() where TData : class;

    }
}
