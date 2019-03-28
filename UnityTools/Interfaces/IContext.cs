using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContext : 
        IMessageBroker,
        IPoolable, 
        IReadOnlyContext,
        IDisposable,
        ILifeTimeContext,
        IContextWriter
    {
        
    }
}
