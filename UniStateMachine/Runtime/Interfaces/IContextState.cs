using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public interface IContextState<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        ILifeTimeContext,
        IPoolable
    {
        bool IsActive { get; }
    }
}
