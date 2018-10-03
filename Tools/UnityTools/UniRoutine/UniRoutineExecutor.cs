using System.Collections;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.UniRoutine
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
