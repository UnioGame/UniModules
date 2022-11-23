

namespace UniModules.UniActors.Runtime.ActorData
{
    using Interfaces;
    using global::UniGame.Core.Runtime;
    using UnityEngine;

    public class ActorModelInfo<TModel> : 
        BaseActorInfo<TModel> 
        where TModel : class,IActorModel, IFactory<TModel>,new()
    {
 
        [SerializeField] private TModel sourceModel;

        protected override TModel CreateDataSource()
        {
            var model = sourceModel.Create();
            return model;
        }

    }
}
