namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IUniTaskExecutor
    {

        UniTask Execute(UniTask actionTask);

        void Stop();

    }
}
