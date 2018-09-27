using System;
using System.Collections;

namespace Tools.UniRoutineTask
{
    public interface IUniRoutine
    {
        IDisposable AddRoutine(IEnumerator enumerator);
        void Update();
    }
}