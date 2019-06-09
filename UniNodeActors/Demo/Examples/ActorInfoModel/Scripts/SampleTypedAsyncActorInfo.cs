using UniGreenModules.UniNodeActors.Runtime;

namespace UniGreenModules.UniNodeActors.Demo.Examples.ActorInfoModel.Scripts
{
    using System;
    using System.Threading.Tasks;
    using Runtime.ActorData;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniNodeActors/Demo/SampleActorInfo",fileName = "SampleActorInfo")]
    public class SampleTypedAsyncActorInfo : AsyncActorModelInfo<SampleActorModel>
    {

        [SerializeField]
        private SampleActorModel model;
        
        protected override async Task<SampleActorModel> CreateDataSource()
        {
            return new SampleActorModel() {
                value = model.value,
                target = model.target,
                modelName = model.modelName
            };    
        }
        
    }
}
