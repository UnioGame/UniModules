namespace UniTools.UniRoutine.Runtime.Interfaces
{
    using System.Collections;

    public interface IUniRoutine
    {
        bool CancelRoutine(int id);
        
        IUniRoutineTask AddRoutine(IEnumerator enumerator, bool moveNextImmediately = true);
        
        void Update();
    }
}