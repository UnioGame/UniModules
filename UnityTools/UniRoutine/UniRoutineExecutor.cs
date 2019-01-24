using System.Collections;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniRoutine
{
    public class UniRoutineExecutor : IContextExecutor<IEnumerator>
    {

        public IDisposableItem Execute(IEnumerator data)
        {
            var disposable = data.RunWithSubRoutines();
            return disposable;
        }

    }
}
