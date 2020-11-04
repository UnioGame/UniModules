namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System.Collections;
    using UniGame.Core.Runtime.Interfaces;

    public interface IUniStateTransition : IValidator<IContext>
    {
        IContextState<IEnumerator> SelectState(IContext context);
    }
}