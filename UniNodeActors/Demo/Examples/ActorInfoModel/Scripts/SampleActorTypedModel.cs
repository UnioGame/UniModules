namespace UniGreenModules.UniNodeActors.Demo.Examples.ActorInfoModel.Scripts
{
    using System;
    using Runtime.ActorData;
    using Runtime.Models;
    using UniCore.Runtime.ObjectPool;
    using UnityEngine;

    [Serializable]
    public class SampleActorModel : ActorModel
    {
        
        public GameObject target;

        public int value;

        public string modelName;

        public override void MakeDespawn()
        {
            if(target)
                GameObject.Destroy(target);

            value = 0;
            modelName = string.Empty;
            
            this.Despawn();
        }
    }
}
