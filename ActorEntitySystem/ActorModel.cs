using System;
using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel
{
    [Serializable]
    public class ActorModel : IActorModel
    {
        #region public methods

        public virtual void Release(){}

        public virtual void Register(IContext context)
        {
            context.Add(this);
        }
        
        #endregion

 
    }
}