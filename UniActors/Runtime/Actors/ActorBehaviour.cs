using System.Collections;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniStateMachine.Runtime.Interfaces;
using UnityEngine;

namespace UniGreenModules.UniActors.Runtime.Actors
{
    public abstract class ActorBehaviour : MonoBehaviour, IContextState<IEnumerator>
    {
        
        public ILifeTime LifeTime { get; protected set; }

        public bool IsActive { get; protected set; }

        public abstract IEnumerator Execute(IContext data);

        public abstract void Exit();
        
        public abstract void Release();

    }
}
