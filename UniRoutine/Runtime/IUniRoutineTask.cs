namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

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