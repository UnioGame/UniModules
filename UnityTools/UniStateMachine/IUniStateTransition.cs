using System.Collections;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Interfaces;

namespace UniStateMachine
{
    public interface IUniStateTransition : IValidator<IContext>
    {
        IContextState<IEnumerator> SelectState(IContext context);
    }
}