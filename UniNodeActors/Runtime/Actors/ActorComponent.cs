namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using Interfaces;
    using UnityEngine;

    public class ActorComponent : BaseActorComponent
    {

        #region inspector data

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField] private IActorInfo<IActorModel> actorInfo;

        #endregion

        protected override IActorModel GetModel()
        {
            var model = actorInfo?.Create();
            return model;
        }

    }
}