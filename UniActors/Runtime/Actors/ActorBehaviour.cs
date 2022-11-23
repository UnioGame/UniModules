using System.Collections;
using UniModules.UniStateMachine.Runtime.Interfaces;
using UnityEngine;

namespace UniModules.UniActors.Runtime.Actors
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using global::UniGame.Core.Runtime;

    public abstract class ActorBehaviour : MonoBehaviour, IContextState<IEnumerator>
    {
        
        public ILifeTime LifeTime { get; protected set; }

        public bool IsActive { get; protected set; }

        public abstract IEnumerator Execute(IContext data);

        public abstract void Exit();
        
        public abstract void Release();

    }
}
