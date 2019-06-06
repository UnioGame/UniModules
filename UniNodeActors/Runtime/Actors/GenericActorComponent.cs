﻿namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using ActorData;
    using Interfaces;
    using UnityEngine;

    public class GenericActorComponent<TModel> : BaseActorComponent
        where TModel : class,IActorModel
    {

        #region inspector data

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField] 
        private TypedAsyncActorInfo<TModel> typedAsyncActorInfo;

        #endregion

        protected override IActorModel GetModel()
        {
            var model = typedAsyncActorInfo.Create();
            return model;
        }

    }
}