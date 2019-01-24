using System;
using System.Collections;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.ActorEntityModel
{
    [Serializable]
    public class ActorModel : IPoolable
    {
        public string Name;

        public IContextState<IEnumerator> Behaviour;
    
        public virtual void Release()
        {
            Behaviour = null;
        }

        public virtual void RegisterContext(IContext context)
        {

            context.Add(this);

        }


    }
}
