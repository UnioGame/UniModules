namespace UniTools.UniRoutine.Runtime
{
    using UniGreenModules.UniCore.Runtime.Common;
    using UniGreenModules.UniCore.Runtime.Extension;

    public struct RoutineItem
    {
        public DisposableAction Disposable;
        public UniRoutineTask   Task;

        public bool MoveNext()
        {
            return Task.MoveNext();
        }

        public void Release()
        {
            Task.Dispose();
            Disposable.Cancel();
        }
    }
}