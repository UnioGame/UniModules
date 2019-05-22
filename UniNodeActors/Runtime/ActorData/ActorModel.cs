namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using Interfaces;
    using UniCore.Runtime.Interfaces;

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