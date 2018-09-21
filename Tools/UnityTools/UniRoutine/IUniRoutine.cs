using System.Collections;

namespace Tools.UniRoutineTask
{
    public interface IUniRoutine
    {
        void AddRoutine(IEnumerator enumerator);
        void Update();
    }
}