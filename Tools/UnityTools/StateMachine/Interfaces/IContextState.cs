using System;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextState<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        IDisposable
    {
        bool IsActive(IContext context);
    }
}
