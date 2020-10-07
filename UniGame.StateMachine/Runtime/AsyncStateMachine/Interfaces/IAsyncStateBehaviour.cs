namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using Cysharp.Threading.Tasks;
    using Runtime.Interfaces;
    

    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{}
    
}