using UniGreenModules.UniNodeActors.Runtime.Interfaces;
using UnityEngine;

namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
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
