namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IAsyncCommand<T>
    {
        UniTask<T> Execute();
    }
    
    public interface IAsyncCommand<T,TValue>
    {
        UniTask<T> Execute(TValue value);
    }
}