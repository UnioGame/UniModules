namespace UniGreenModules.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IUniRoutineTask : IEnumerator<IEnumerator>, IPoolable
    {
        int Id { get; }
        ILifeTime LifeTime { get; }
        bool IsCompleted { get; }
        IEnumerator Current { get; }
        void Pause();
        void Unpause();
        void Complete();
        void Dispose();
    }
}