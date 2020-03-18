namespace UniGreenModules.UniActors.Demo.Examples.ActorInfoModel.Scripts
{
    using System;
    using Runtime.Models;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UnityEngine;

    [Serializable]
    public class SampleActorModel : ActorModel
    {
        
        public GameObject target;

        public int value;

        public string modelName;

        public override void MakeDespawn()
        {
            this.DespawnObject(Release);
        }

        public void Release()
        {
            if(target)
                GameObject.Destroy(target);

            value     = 0;
            modelName = string.Empty;
        }
    }
}
