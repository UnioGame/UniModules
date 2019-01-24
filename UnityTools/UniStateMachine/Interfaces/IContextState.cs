using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IContextState<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        IDisposable
    {
        bool IsActive(IContext context);

        ILifeTime GetLifeTime(IContext context);
    }
}
