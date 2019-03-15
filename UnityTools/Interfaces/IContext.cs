using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.ObjectPool.Scripts;
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
