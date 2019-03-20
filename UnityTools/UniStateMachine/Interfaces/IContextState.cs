using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IContextState<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
        bool IsActive { get; }
    }
}
