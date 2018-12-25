using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx;

[Serializable]
public class ActorModel : IPoolable
{
    private static int _actorId;
    
    public string Name;
    
    public IntReactiveProperty Id = new IntReactiveProperty();

    public ReactiveProperty<IContextState<IEnumerator>> Behaviour = new ReactiveProperty<IContextState<IEnumerator>>();

    
    public virtual void Release()
    {
        Id.Value = 0;
        Behaviour.Value = null;
    }

    public virtual void AddContextData(IContext context)
    {

        context.Add(this);

    }
    
    protected void Initialize()
    {
        _actorId++;
        Id.Value = _actorId;
    }

}
