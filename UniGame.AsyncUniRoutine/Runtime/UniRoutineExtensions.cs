namespace UniModules.UniGame.AsyncUniRoutine.Runtime
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;

    public static class UniRoutineExtensions
    {
        public static UniTaskYieldInstruction AsYieldInstruction(this UniTask task)
        {
            return new UniTaskYieldInstruction(task);
        }

        public static UniTaskYieldInstruction<T> AsYieldInstruction<T>(this UniTask<T> task)
        {
            return new UniTaskYieldInstruction<T>(task);
        }

        public static TaskYieldInstruction AsYieldInstruction(this Task task)
        {
            return new TaskYieldInstruction(task);
        }

        public static TaskYieldInstruction<T> AsYieldInstruction<T>(this Task<T> task)
        {
            return new TaskYieldInstruction<T>(task);
        }
    }
}