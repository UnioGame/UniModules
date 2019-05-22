using UniGreenModules.UniNodeActors.Runtime;

namespace UniGreenModules.UniNodeActors.Demo.Examples.ActorInfoModel.Scripts
{
    using Runtime.ActorData;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniNodeActors/Demo/SampleActorInfo",fileName = "SampleActorInfo")]
    public class SampleTypedActorInfo : TypedActorInfo<SampleActorModel>
    {
        
        protected override SampleActorModel CreateDataSource(SampleActorModel model)
        {
            return new SampleActorModel() {
                value = model.value,
                target = model.target,
                modelName = model.modelName
            };    
        }
        
    }
}
