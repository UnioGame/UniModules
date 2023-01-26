namespace UniModules.UniActors.Runtime.Models
{
    using System;
    using Interfaces;
    using global::UniGame.Core.Runtime;

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