using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx;

public class ActorModel
{

    public string Name;
    
    public ReactiveProperty<IContextState<IEnumerator>> Behaviour;

    public virtual void AddContextData(IContext context)
    {

        context.Add(this);

    }

}
