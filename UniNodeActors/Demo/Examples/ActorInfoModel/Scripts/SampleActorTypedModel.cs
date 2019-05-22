using UniGreenModules.UniNodeActors.Runtime;

namespace UniGreenModules.UniNodeActors.Demo.Examples.ActorInfoModel.Scripts
{
    using System;
    using Runtime.ActorData;
    using UnityEngine;

    [Serializable]
    public class SampleActorModel : ActorModel
    {
        
        public GameObject target;

        public int value;

        public string modelName;
        
    }
}
