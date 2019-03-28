

namespace Modules.UniTools.ActorEntitySystem
{
    using UniModule.UnityTools.ActorEntityModel;
    using UniModule.UnityTools.Interfaces;
    using UnityEngine;

    public class ActorModelInfo<TModel> : 
        ActorInfo
        where TModel : class,IActorModel,
            IFactory<TModel>,new()
    {
 
        [SerializeField] private TModel sourceModel;

        protected override IActorModel CreateDataSource()
        {
            var model = sourceModel.Create();
            return model;
        }

    }
}
