using UniGreenModules.UniNodeActors.Runtime;

namespace UniGreenModules.UniNodeActors.Demo.Examples.ActorInfoModel.Scripts
{
    using Runtime.ActorData;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniNodeActors/Demo/SampleActorInfo",fileName = "SampleActorInfo")]
    public class SampleTypedAsyncActorInfo : TypedAsyncActorInfo<SampleActorModel>
    {
        
        protected override SampleActorModel CreateDataSource()
        {
            return new SampleActorModel() {
                value = sourceModel.value,
                target = sourceModel.target,
                modelName = sourceModel.modelName
            };    
        }
        
    }
}
