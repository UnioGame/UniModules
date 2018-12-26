using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx;

[Serializable]
public class ActorModel : IPoolable
{
    public string Name;
    
    public ReactiveProperty<IContextState<IEnumerator>> Behaviour = new ReactiveProperty<IContextState<IEnumerator>>();
    
    public virtual void Release()
    {
        Behaviour.Value = null;
    }

    public virtual void RegisterContext(IContext context)
    {

        context.Add(this);

    }


}
