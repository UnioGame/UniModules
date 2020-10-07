namespace UniModules.UniActors.Runtime.Actors
{
    using Interfaces;
    using UnityEngine;

    public class GenericActorComponent<T,TModel> : BaseActorComponent
        where T : IActorInfo<TModel>
        where TModel : IActorModel
    {

        [SerializeField]
        private T actorInfo;

        protected override IActorModel GetModel()
        {
            var model = actorInfo.Create();
            return model;
        }
    
    }
}
