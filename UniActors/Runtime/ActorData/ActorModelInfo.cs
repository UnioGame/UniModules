

namespace UniGreenModules.UniActors.Runtime.ActorData
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;
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
