

namespace UniModules.UniActors.Runtime.ActorData
{
    using Interfaces;
    using UniGame.Core.Runtime.Interfaces;
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
