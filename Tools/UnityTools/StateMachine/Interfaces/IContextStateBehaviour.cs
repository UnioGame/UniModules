using System;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextStateBehaviour<out TAwaiter> : 
        IStateBehaviour<IContext, TAwaiter>,
        IDisposable
    {
    }
}
