namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using Interfaces;
    using UniCore.Runtime.Interfaces;

    [Serializable]
    public abstract class ActorModel : IActorModel
    {
        #region public methods


        public virtual void Register(IContext context)
        {
            context.Add(this);
        }

        public abstract void MakeDespawn();
        
        #endregion

    }
}