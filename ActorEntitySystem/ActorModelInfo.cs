
namespace Modules.UniTools.ActorEntitySystem
{
    using UniModule.UnityTools.ActorEntityModel;
    using UniModule.UnityTools.UniPool.Scripts;
    using UnityEngine;

    public class ActorModelInfo<TModel> : ActorInfo
        where TModel : ActorModel<TModel>,new()
    {
        [SerializeField]
        private TModel sourceModel;
    
        protected override IActorModel CreateDataSource()
        {
            var model = ClassPool.Spawn<TModel>();
            model.Initialize(sourceModel);
            return model;
        }
    }
}
