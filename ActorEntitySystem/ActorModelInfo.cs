
namespace Modules.UniTools.ActorEntitySystem
{
    using UniModule.UnityTools.ActorEntityModel;
    using UniModule.UnityTools.UniPool.Scripts;
    using UnityEngine;

    public class ActorModelInfo<TModel> : 
        ActorInfo
        where TModel : class,IActorModel,new()
    {
 
        [SerializeField] private TModel sourceModel;

        protected override IActorModel CreateDataSource()
        {
            var model = ClassPool.Spawn<TModel>();
            SetUpData(ref model,sourceModel);
            return model;
        }

        protected virtual void SetUpData(ref TModel data,TModel source){}
    }
}
