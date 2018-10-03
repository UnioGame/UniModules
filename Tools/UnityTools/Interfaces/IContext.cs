using Assets.Tools.Utils;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IContext : IPoolable
    {

        TData Get<TData>();
        bool Remove<TData>();
        void Add<TData>(TData data);

    }
}
