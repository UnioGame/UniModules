using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.Core.Runtime.Extension
{
    public static class UniTaskExtensions
    {
        public static bool IsCompleted(this UniTask task) => task.Status.IsCompleted();
    
        public static bool IsCompleted<T>(this UniTask<T> task) => task.Status.IsCompleted();
    }
}
