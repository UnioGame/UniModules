using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;

namespace UniModule.UnityTools.ActorEntityModel
{
    public interface IContextDataSource
    {
        void Register(IContext context);
    }
}