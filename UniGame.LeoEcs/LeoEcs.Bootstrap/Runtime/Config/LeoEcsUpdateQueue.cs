using System;
using UnityEngine;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    [Serializable]
    public class LeoEcsUpdateQueue
    {
        public string OrderId = string.Empty;
        
        [SerializeReference]
        public ILeoEcsUpdateOrderProvider Factory;
    }
}