namespace UniGreenModules.UniRoutine.Runtime
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;

    public class UniRoutineExecutor : IContextExecutor<IEnumerator>
    {

        public IDisposableItem Execute(IEnumerator data)
        {
            var disposable = data.RunWithSubRoutines();
            return disposable;
        }

    }
}
