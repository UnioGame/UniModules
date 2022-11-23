namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System.Collections;
    using global::UniGame.Core.Runtime;

    public interface IUniStateTransition : IValidator<IContext>
    {
        IContextState<IEnumerator> SelectState(IContext context);
    }
}