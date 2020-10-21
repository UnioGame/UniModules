namespace UniModules.UniActors.Runtime.Models
{
    using System;
    using Interfaces;
    using UniGame.Core.Runtime.Interfaces;

    [Serializable]
    public abstract class ActorModel : IActorModel
    {
        
        #region public methods


        public virtual void Register(IContext context)
        {
            context.Publish(this);
        }

        public abstract void MakeDespawn();
        
        #endregion

    }
}