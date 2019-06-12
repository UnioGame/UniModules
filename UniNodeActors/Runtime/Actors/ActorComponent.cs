namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using ActorData;
    using Interfaces;
    using UnityEngine;

    public class ActorComponent : BaseActorComponent
    {

        #region inspector data

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField] 
        private ActorInfo actorInfo;

        #endregion

        protected override IActorModel GetModel()
        {
            var model = actorInfo?.Create();
            return model;
        }

    }
}