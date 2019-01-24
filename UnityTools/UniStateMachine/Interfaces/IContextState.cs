using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextState<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        IDisposable
    {
        bool IsActive(IContext context);

        ILifeTime GetLifeTime(IContext context);
    }
}
