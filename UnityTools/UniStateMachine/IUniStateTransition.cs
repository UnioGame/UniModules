using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace UniStateMachine
{
    public interface IUniStateTransition : IValidator<IContext>
    {
        IContextState<IEnumerator> SelectState(IContext context);
    }
}