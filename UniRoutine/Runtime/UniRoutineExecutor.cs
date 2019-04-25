namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using UniModule.UnityTools.Interfaces;

    public class UniRoutineExecutor : IContextExecutor<IEnumerator>
    {

        public IDisposableItem Execute(IEnumerator data)
        {
            var disposable = data.RunWithSubRoutines();
            return disposable;
        }

    }
}
