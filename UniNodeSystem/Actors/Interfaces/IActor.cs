using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel
{
    public interface IActor : IBehaviourObject
    {
        IContext Context { get; }
    }
}