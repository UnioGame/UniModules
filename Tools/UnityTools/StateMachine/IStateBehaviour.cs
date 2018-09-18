
using System;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateBehaviour<TResult> : IRoutine<TResult>, IDisposable
    {
        void Stop();
    }

}